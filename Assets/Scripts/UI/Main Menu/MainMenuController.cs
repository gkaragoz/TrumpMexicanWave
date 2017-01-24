using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
	public void LoadGameScene()
	{
		AudioController.Instance.PlayEffect( Effect.UI_TAP );
		SceneManager.LoadScene( "MainScene" );
	}

	// Use this for initialization
	void Start ()
	{
        AudioController.Instance.PlayEffectInLoop(Effect.HAPPY_OPENING, 0.025f);
        Debug.Log("[MainMenu] Loaded main menu.");
	}
}
