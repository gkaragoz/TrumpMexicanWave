using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private CameraController _camera;

	[SerializeField]
	private MexicanWaver _mexiController;

	private List<Culture> _angryCultures;

    public Text Txt_PlayerLife;

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
                //Call Death Method!
            }
        }
    }

    private void StartGame()
	{
		ResetAudience();
		StartCoroutine( WaitForTrumpTalks( 4f ) );
	}

	void Start ()
	{
		_angryCultures = new List<Culture>();
		_angryCultures.Add( Culture.MUSLIM );

		StartGame();
	}

	private void ResetAudience()
    {
        //Reset life for new wave.
        Life = 10;

        //Set up the people
        _mexiController.InitWave(_angryCultures);
	}

    IEnumerator WaitForTrumpTalks(float time)
    {
        //Trump talks.
        yield return new WaitForSeconds(time);

		//Tell the camera to zoom in...
		_camera.StartCameraZoom();
    }
}
