using UnityEngine;
using System.Collections;

public enum Culture
{
	CAUCASIAN = 0,
	MEXICAN = 1,
	AFRICANAMERICAN = 2,
	LIBERAL = 3,
	MUSLIM = 4
}

public class ViewerController : MonoBehaviour
{

	[SerializeField]
	private Culture _culture;
	public Culture ActiveCulture
	{
		get
		{
			return _culture;
		}
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void StandUp()
	{
		LeanTween.moveY(
			gameObject, 
			transform.position.y + 0.5f, 
			Random.Range(0.25f, 0.5f));
	}

	public void SitDown()
	{
		LeanTween.moveY(
			gameObject,
			transform.position.y - 0.5f,
			Random.Range(0.25f, 0.5f));
	}
}
