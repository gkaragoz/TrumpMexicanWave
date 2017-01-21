using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private CameraController _camera;

	[SerializeField]
	private MexicanWaver _mexiController;

	private void StartGame()
	{
		ResetAudience();
		StartCoroutine( WaitForTrumpTalks( 4f ) );

	}

	void Start ()
	{
		StartGame();
	}

	void Update ()
	{

	}

	private void ResetAudience()
	{
		//Set up the people
		_mexiController.InitWave();
	}

    IEnumerator WaitForTrumpTalks(float time)
    {
        //Trump talks.
        yield return new WaitForSeconds(time);

		//Tell the camera to zoom in...
		_camera.StartCameraZoom();
    }
}
