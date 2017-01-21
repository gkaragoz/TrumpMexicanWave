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

	public float Delay = 0.2f;
	public float StandTime = 0.25f;
	public float SitTime = 0.25f;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter2D(Collider2D thingThatHitMe)
	{
		//Debug.Log("Ow, my face");
		StandUpThenDown();
	}

	public void StandUpThenDown()
	{
		LeanTween.moveY(
			gameObject, 
			transform.position.y + 0.25f, 
			Random.Range(StandTime, StandTime + StandTime * 1.2f))
			.setOnComplete( SitDown );
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
		Debug.Log("THER SEE ME SITTING");
		LeanTween.moveY(
			gameObject,
			transform.position.y - 0.25f,
			SitTime)
			.setDelay( Delay );
	}
}
