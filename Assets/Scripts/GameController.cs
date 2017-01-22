using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private CameraController _camera;

	[SerializeField]
	private MexicanWaver _mexiController;

	private List<Culture> _angryCultures;
	private int _maxAngries = 20;
	private readonly int MAX_ANGRIES_POSSIBLE = 40;

    public Text Txt_PlayerLife;

	[SerializeField]
	private Button _fader;

    [SerializeField]
    private int _life;

	private bool _gameInProgress;

    private bool _gameOver;

    public int Life
    {
        get
        {
            return _life;
        }

        set
        {
            _life = value;
            Txt_PlayerLife.text = " Life: " + _life;

            if (Life <= 0 && _gameInProgress)
            {
                //_camera.Reset();
                //StartGame();
				Debug.Log("Someone set life to 0, aww sheet");
                //SceneManager.LoadScene("MainMenu");

                _gameOver = true;
                DoTransition();
            }
        }
    }

    private void StartGame()
	{
        Camera.main.orthographicSize = 5;
		_fader.interactable = false;
		ResetAudience();

        if (_gameInProgress)
            CallClickableNewspaper();

        _gameOver = false;
		_gameInProgress = true;
	}

	void Start ()
	{
		_angryCultures = new List<Culture>();

		ESCALATE();

		_camera.SetOnGameEnd(DoTransition);

		StartGame();
	}

	//We're making all caps function names great again
	void ESCALATE()
	{
		_maxAngries += Random.Range(5, 16);

		if(_maxAngries > MAX_ANGRIES_POSSIBLE)
			_maxAngries = MAX_ANGRIES_POSSIBLE;

		//5 is the max amount of cultures that can hate trump
		//...I know, right?
		if(_angryCultures.Count >= 5)
			return;

		//Index 0 in the Culture enum is the neutral guy
		//So we start at 1, and end at 5
		int randomCulture = Random.Range(1,6);
		var castedForm = (Culture) randomCulture;

		while( _angryCultures.Contains( castedForm ) && castedForm == Culture.LIBERAL )
		{
			randomCulture++;

			//If we're above the max int in the culture enum
			//wrap around back to 1
			if( randomCulture > 5 )
				randomCulture = 1;

			castedForm = (Culture) randomCulture;
		}

		_angryCultures.Add( castedForm );

		Debug.Log("[GameController] People who consider themselves to be " + castedForm + " now despise Trump!");
	}

	private void DoTransition()
	{
		_gameInProgress = false;
		
        CallClickableNewspaper();

		ESCALATE();
	}

    public void CallClickableNewspaper()
    {
        //Fade out to black
        _camera.StartFadeInCor();
        _fader.interactable = true;

        //We don't want to call this from camera
        //So we'll just use the magic number 1.0f
        //which is the duration of the fade out 
        //coroutine
        LeanTween.delayedCall(1.0f,
                () =>
                {
                    if (Newspaper.Instance != null)
                    {
                        if (_gameOver)
                        {
                            _camera.StartTranslate = false;
                            Txt_PlayerLife.text = "";
                            Newspaper.Instance.Show(3f, "Game over!", "You loss!");
                        }
                        else //Depends on haters cultures.
                            Newspaper.Instance.Show(3f, "Trump does it again!", "Blacks outraged!");
                    }
                });
    }

	//After fading out, the user taps the
	//newspaper to start the next wave
	public void OnNewspaperTap()
	{
        if (_gameOver)
        {
            Newspaper.Instance.Hide();
            SceneManager.LoadScene("MainMenu");
            return;
        }

        _gameInProgress = false;
		StartGame();
        StartCoroutine(WaitForTrumpTalks(4f));

        _camera.StartFadeOutCor();

        if (Newspaper.Instance != null)
                Newspaper.Instance.Hide();
	}

	private void ResetAudience()
    {
        //Reset life for new wave.
        Life = 2;

		//Clear people (if any)
		_mexiController.ClearWave();

        //Set up the people
        _mexiController.InitWave(_angryCultures, _maxAngries);
		Debug.Log("[GameController] Audience resetted!");
	}

    IEnumerator GetNewspaperAnim()
    {

        yield return new WaitForSeconds(5);
    }

    IEnumerator WaitForTrumpTalks(float time)
    {
        //Trump talks.
        yield return new WaitForSeconds(time);

		//Tell the camera to zoom in...
		_camera.StartCameraZoom();
    }
}
