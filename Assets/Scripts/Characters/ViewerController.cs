using UnityEngine;
using System.Collections;

public enum Culture
{
	CAUCASIAN = 0,
	MEXICAN = 1,
	AFRICANAMERICAN = 2,
	LIBERAL = 3,
	MUSLIM = 4,
	WOMEN = 5
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

	public bool HatesTrump = false;

	void Update ()
	{
        if(Mathf.Abs(transform.position.x - Camera.main.transform.position.x + 4.5f) <= 2.34f && HatesTrump == true && Camera.main.GetComponent<CameraController>().StartTranslate)
        {
            Debug.Log(Mathf.Abs(transform.position.x - Camera.main.transform.position.x + 4.5f));
            Destroy(gameObject);
        }
	}

	void OnTriggerEnter2D(Collider2D thingThatHitMe)
	{
		//Debug.Log("Ow, my face");
		if( !HatesTrump )
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
		LeanTween.moveY(
			gameObject,
			transform.position.y - 0.25f,
			SitTime)
			.setDelay( Delay );
	}
}
