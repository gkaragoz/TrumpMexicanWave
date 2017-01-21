using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private CameraController _camera;

	[SerializeField]
	private MexicanWaver _mexiController;

	private List<Culture> _angryCultures;

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

	void Update ()
	{

	}

	private void ResetAudience()
	{
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
