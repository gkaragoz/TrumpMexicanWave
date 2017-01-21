using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
	public void LoadGameScene()
	{
		SceneManager.LoadScene( "MainScene" );
	}

	// Use this for initialization
	void Start ()
	{
		Debug.Log("[MainMenu] Loaded main menu.");
	}
}
