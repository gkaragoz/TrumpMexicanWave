using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public Transform Target;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    public bool isFading;
    public MexicanWaver MW;
    public GameObject StartPosition;
    public GameObject EndPosition;
    public Image FADE_IN_OUT;
    public bool StartTranslate;

	void Start () {
        StartTranslate = false;
        isFading = false;

        StartCoroutine(WaitForTrumpTalks(4));
	}
	
	void Update () {
        if (!isFading)
        {
            if (Camera.main.transform.position.x >= EndPosition.transform.position.x)
            {
                isFading = true;
                StartTranslate = false;
                StartCoroutine(CameraZoomOut());
            }
            if (StartTranslate)
            {
                if (Target != null)
                {
                    Vector3 point = Camera.main.WorldToViewportPoint(Target.position);
                    Vector3 delta = Target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                    Vector3 destination = transform.position + delta + Vector3.right;
                    transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                }
            }
        }
	}

    IEnumerator NewspaperComes()
    {
        //Newspaper animation.
        yield return new WaitForSeconds(2);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        LeanTween.value(FADE_IN_OUT.gameObject, FadeIn, 0f,1f, 1f);
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        //Reset camera.
        Camera.main.transform.position = new Vector3(0, 0, -10);

        LeanTween.value(FADE_IN_OUT.gameObject, FadeOut, 1f,0f, 1f);
        yield return new WaitForSeconds(1);

        LeanTween.cancelAll();
        isFading = false;

        StartCoroutine(WaitForTrumpTalks(4));
    }

    //Fade IN animation. Increase the alpha channel on FADE_IN_OUT Image. 
    void FadeIn(float val)
    {
        FADE_IN_OUT.color = new Color(FADE_IN_OUT.color.r, FADE_IN_OUT.color.g, FADE_IN_OUT.color.b, val);
    }
    //Fade OUT animation. Decrease the alpha channel on FADE_IN_OUT Image.
    void FadeOut(float val)
    {
        FADE_IN_OUT.color = new Color(FADE_IN_OUT.color.r, FADE_IN_OUT.color.g, FADE_IN_OUT.color.b, val);
    }

    IEnumerator CameraFadeLeft()
    {
        LeanTween.moveX(gameObject, StartPosition.transform.position.x, 2).setEase(LeanTweenType.linear);

        yield return new WaitForSeconds(2);

        StartTranslate = true;

        StartCoroutine(MW.LeanTween_StandUp());
        StartCoroutine(MW.LeanTween_SitDown());
    }

    IEnumerator CameraZoomIn()
    {
        LeanTween.moveY(gameObject, Camera.main.transform.position.y + 0.75f, 2);
        LeanTween.value(gameObject, animateOrthographicSize, 5, 3, 2);

        yield return new WaitForSeconds(2);

        StartCoroutine(CameraFadeLeft());
    }

    IEnumerator CameraZoomOut()
    {
        LeanTween.moveY(gameObject, Camera.main.transform.position.y - 0.75f, 1);
        LeanTween.value(gameObject, animateOrthographicSize, 3, 5, 1);

        yield return new WaitForSeconds(1);

        StartCoroutine(NewspaperComes());
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

    public void Tracking(GameObject person)
    {
        Target.transform.position = new Vector3(person.transform.position.x, MW.X_StartPosition.transform.position.y, Target.transform.position.z);
        //Camera.main.transform.position = new Vector3(person.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Target.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), 2f);
    }
}
