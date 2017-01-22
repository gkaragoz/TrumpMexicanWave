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

    public Text Txt_PlayerLife;

	[SerializeField]
	private Button _fader;

    [SerializeField]
    private int _life;

	private bool _gameInProgress;

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
				SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void StartGame()
	{
        Camera.main.orthographicSize = 5;
		_fader.interactable = false;
		ResetAudience();
		StartCoroutine( WaitForTrumpTalks( 4f ) );

		_gameInProgress = true;
	}

	void Start ()
	{
		_angryCultures = new List<Culture>();
		_angryCultures.Add( Culture.MUSLIM );

		_camera.SetOnGameEnd(DoTransition);

		StartGame();
	}

	private void DoTransition()
	{
		_gameInProgress = false;
		
		//Fade out to black
		_camera.StartFadeInCor();
		_fader.interactable = true;
	}

	//After fading out, the user taps the
	//newspaper to start the next wave
	public void OnNewspaperTap()
	{
		_gameInProgress = false;
		StartGame();
		_camera.StartFadeOutCor();
	}

	private void ResetAudience()
    {
        //Reset life for new wave.
        Life = 2;

		//Clear people (if any)
		_mexiController.ClearWave();

        //Set up the people
        _mexiController.InitWave(_angryCultures);
		Debug.Log("[GameController] Audience resetted!");
	}

    IEnumerator WaitForTrumpTalks(float time)
    {
        //Trump talks.
        yield return new WaitForSeconds(time);

		//Tell the camera to zoom in...
		_camera.StartCameraZoom();
    }
}
