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

            if (Life <= 0)
            {
                //_camera.Reset();
                //StartGame();
				SceneManager.LoadScene("MainMenu");
            }
        }
    }

    private void StartGame()
	{
		_fader.interactable = false;
		ResetAudience();
		StartCoroutine( WaitForTrumpTalks( 4f ) );
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
		//Fade out to black
		_camera.StartFadeInCor();
		_fader.interactable = true;
	}

	//After fading out, the user taps the
	//newspaper to start the next wave
	public void OnNewspaperTap()
	{
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
