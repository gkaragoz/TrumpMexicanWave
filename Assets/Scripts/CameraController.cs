using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public MexicanWaver MW;
    public GameObject StartPosition;
    public bool StartTranslate;

	void Start () {
        StartTranslate = false;

        StartCoroutine(WaitForTrumpTalks(4));
	}
	
	void Update () {
        if (StartTranslate)
            transform.Translate(2 * Time.deltaTime, 0, 0);
	}

    IEnumerator CameraFadeLeft()
    {
        LeanTween.moveX(gameObject, StartPosition.transform.position.x, 2).setEase(LeanTweenType.linear);

        yield return new WaitForSeconds(2);

        StartTranslate = true;

        StartCoroutine(MW.iTween_StandUp());
        StartCoroutine(MW.iTween_SitDown());
    }

    IEnumerator CameraZoomIn()
    {
        LeanTween.moveY(gameObject, Camera.main.transform.position.y + 0.75f, 2);
        LeanTween.value(gameObject, animateOrthographicSize, 5, 3, 2);

        yield return new WaitForSeconds(2);
        StartCoroutine(CameraFadeLeft());
    }

    //Apply the ortographic size of zoom to the main camera:
    void animateOrthographicSize(float newOrthographicSize)
    {
        Camera.main.orthographicSize = newOrthographicSize;
    }

    IEnumerator WaitForTrumpTalks(float time)
    {
        //Trump talks.
        yield return new WaitForSeconds(time);

        StartCoroutine(CameraZoomIn());
    }
}
