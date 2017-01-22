using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Newspaper : MonoBehaviour
{

	public static Newspaper Instance
	{
		get;
		private set;
	}

	[SerializeField]
	private Text _topText;
	[SerializeField]
	private Text _flavorText;

	// Use this for initialization
	void Start ()
	{
		Instance = this;

		//Hide newspaper by default
		gameObject.SetActive( false );

		transform.position = Vector3.zero;
	}

	void Show (float duration = 1f, string topText = "Trump eats baby", string flavorText = "'Wow, tasty baby!'")
	{
		//Cancel all previously active tweens
		LeanTween.cancelAll();

		//Reset transform values
		ResetTransform();

		Vector3 targetPos = transform.localPosition;
		transform.localPosition += new Vector3(Screen.width, Screen.height, 0);

		//Set texts (if any)
		_topText.text = topText;
		_flavorText.text = flavorText;

		gameObject.SetActive(true);

		//The tweens
		LeanTween.scale( gameObject, Vector3.one, duration )
			.setEaseOutQuad()
			.setOnComplete( PersistentZoom );
		LeanTween.rotateAround( gameObject, Vector3.forward, 360f * 8, duration )
			.setEaseOutCirc();
		LeanTween.moveLocal( gameObject, targetPos, duration )
			.setEaseOutCirc();

	}

	private void ResetTransform()
	{
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one * 0.1f;
		transform.rotation = Quaternion.Euler(0,0,0);
	}

	public void Hide()
	{
		LeanTween.cancelAll();
		ResetTransform();
		gameObject.SetActive( false );
	}

	public void PersistentZoom()
	{
		LeanTween.scale( gameObject, Vector3.one * 1.5f, 30f );
	}
}
