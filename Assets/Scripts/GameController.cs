using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score;

	[SerializeField]
    private GameObject[] _uiCulturePrefabs;
	private List<GameObject> _uiCultureGOs;

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

	private string _topNPText;
	private string _flavorNPText;

	//For trump talks
	private Culture _lastAngryCulture;

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
        score = 0;

		_angryCultures = new List<Culture>();

		ESCALATE();

		_camera.SetOnGameEnd(DoTransition);

		StartGame();
	}

	//We're making all caps function names great again
	void ESCALATE()
	{
		_maxAngries += Random.Range(1, 16);

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
		_lastAngryCulture = castedForm;

		_topNPText = TextDictionary.CultureLines[ castedForm ];
		_flavorNPText = TextDictionary.CultureSubLines[ castedForm ];	

		Debug.Log("[GameController] People who consider themselves to be " + castedForm + " now despise Trump!");
	}

	private void DoTransition()
	{
		_gameInProgress = false;
		_life = 10;
		
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
                            Newspaper.Instance.Show(3f, "Game over! Your score is: " + score, "Clinton wins! Angels weep!");

                            AudioController.Instance.PlayEffect(Effect.FAIL_TRUMPET);
                        }
                        else //Depends on haters cultures.
                        {
                            AudioController.Instance.PlayEffect(Effect.FANFARE_2, 0.1f);

                            Newspaper.Instance.Show(3f, _topNPText, _flavorNPText);

#region Angries display
							_uiCultureGOs = new List<GameObject>();

							//Start spawning here
							Vector3 spawnPoint = new Vector3( Screen.width, 0f /*Screen.height * -0.5f*/ /* - _uiCulturePrefabs[0].GetComponent<Image>().sprite.rect.height * 0.75f*/, 0f );

                            foreach(Culture angry in _angryCultures)
                            {
								GameObject prefab = _uiCulturePrefabs[(int)angry];
                                GameObject hater = Instantiate( prefab );

								hater.transform.SetParent( GameObject.FindObjectOfType<Canvas>().transform, true );

								var rect = hater.GetComponent<Image>().sprite.rect;

								hater.transform.position = spawnPoint;
								hater.transform.localScale = Vector3.one;

								//LeanTween.moveLocalX( hater, Screen.width * -1.25f, 10f).setEaseInOutCirc();
								LeanTween.moveX( hater, spawnPoint.x - Screen.width, 10f).setEaseOutCirc();
								_uiCultureGOs.Add( hater );

								spawnPoint += rect.width * 0.5f * Vector3.right;

								//LeanTween.delayedCall( 3f, () => Destroy(hater) );
								Debug.Log("SPAWNED A DUDE BREH");
                            }
#endregion
                        }
                    }
                });
    }

	//After fading out, the user taps the
	//newspaper to start the next wave
	public void OnNewspaperTap()
	{
		foreach( var hater in _uiCultureGOs )
		{
			LeanTween.cancel( hater );
			Destroy(hater);
		}

		_uiCultureGOs.Clear();

        if (_gameOver)
        {
            Newspaper.Instance.Hide();
            SceneManager.LoadScene("MainMenu");
            return;
        }

        AudioController.Instance.PlayEffect(Effect.APPLAUSE_1, 0.1f);

        _gameInProgress = false;
		StartGame();
        StartCoroutine(WaitForTrumpTalks(4f));
        _camera.Reset();
        _camera.StartFadeOutCor();

        if (Newspaper.Instance != null)
                Newspaper.Instance.Hide();
	}

	private void ResetAudience()
    {
        //Reset life for new wave.
        Life = 10;

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
		switch( _lastAngryCulture )
		{
			case Culture.MEXICAN:
				AudioController.Instance.PlayEffect( Effect.TRUMP_MEXICANS_1 );
				break;

			case Culture.AFRICANAMERICAN:
				AudioController.Instance.PlayEffect( Effect.TRUMP_AFRICANAMERICANS_1 );
				break;

			case Culture.MUSLIM:
				AudioController.Instance.PlayEffect( Effect.TRUMP_MUSLIMS_1 );
				break;

			case Culture.LIBERAL:
				AudioController.Instance.PlayEffect( Effect.TRUMP_LIBERALS_1 );
				break;

			case Culture.WOMEN:
				AudioController.Instance.PlayEffect(Effect.TRUMP_WOMEN_1, 8f);
				break;
		}
        //Trump talks.
        yield return new WaitForSeconds(time);

		//Tell the camera to zoom in...
		_camera.StartCameraZoom();
    }
}
