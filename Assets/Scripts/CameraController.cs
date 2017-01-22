using UnityEngine;
using System;
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

	private bool _gameRunning = false;

	//THIS IS HERE, NOW
	//DO NOT JUDGE US
	//WE ARE NOT HORRIBLE PEOPLE
	//JUST HORRIBLE PROGRAMMERS
	//SSH, NO TEARS NOW, ONLY DREAMS
	private Action _onGameEnd;

	void Start () {

		Reset();

        GameObject.Find("GameManager").GetComponent<GameController>().CallClickableNewspaper();
        //StartCoroutine(WaitForTrumpTalks(4));
	}

	public void Reset()
	{
        StartTranslate = false;
        //isFading = false;

		Target.transform.position = Vector3.up * Screen.height * 2f;
        Camera.main.transform.position = new Vector3(0, 0, -10);
	}
	public void SetOnGameEnd(Action toSet)
	{
		_onGameEnd = toSet;
	}
	
	void Update () {

		if(!_gameRunning)
			return;

        if (!isFading)
        {
            if (Camera.main.transform.position.x >= EndPosition.transform.position.x)
            {

				if(_onGameEnd != null)
				{
					_gameRunning = false;
					StartTranslate = false;

					_onGameEnd();
				}
				else
					Debug.LogError("[ERROR] You didn't set _onGameEnd, ya dingus!");
            }
            if (StartTranslate)
            {
				Tracking();

                if (Target != null)
                {
                    Vector3 point = Camera.main.WorldToViewportPoint(Target.position);
                    Vector3 delta = Target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                    Vector3 destination = transform.position + delta + (Vector3.right * 0.5f);
                    transform.position = Vector3.SmoothDamp(transform.position, destination + Vector3.right * 0.5f, ref velocity, dampTime);
                }
            }
        }
	}

    IEnumerator NewspaperComes()
    {
        //Newspaper animation.
        yield return new WaitForSeconds(2);

        StartCoroutine( FadeIn(_onGameEnd) );
    }

    IEnumerator FadeIn(Action onFinish = null)
    {
        isFading = true;
        LeanTween.value(FADE_IN_OUT.gameObject, FadeIn, 0f,1f, 1f);
        yield return new WaitForSeconds(1.5f);
        isFading = false;

		if( onFinish != null )
			onFinish();
    }

	public void StartFadeOutCor()
	{
		StartCoroutine( FadeOut() );
	}

	public void StartFadeInCor()
	{
		StartCoroutine( FadeIn() );
	}

    IEnumerator FadeOut()
    {
        isFading = true;
        LeanTween.value(FADE_IN_OUT.gameObject, FadeOut, 1f,0f, 1f);
        yield return new WaitForSeconds(1);
        isFading = false;
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

        int random = UnityEngine.Random.Range(0, 3);

        switch (random)
        {
            case 0:
                AudioController.Instance.PlayEffect(Effect.APPLAUSE_1);
                break;
            case 1:
                AudioController.Instance.PlayEffect(Effect.APPLAUSE_2);
                break;
            case 2:
                AudioController.Instance.PlayEffect(Effect.APPLAUSE_3);
                break;
        }

        yield return new WaitForSeconds(2);

        StartTranslate = true;
        Target.transform.position = transform.position;
    }

    IEnumerator CameraZoomIn()
    {
        LeanTween.moveY(gameObject, Camera.main.transform.position.y + 0.6f, 2);
        LeanTween.value(gameObject, animateOrthographicSize, 5, 4.42f, 2);

        yield return new WaitForSeconds(2);

        StartCoroutine(CameraFadeLeft());
    }

    IEnumerator CameraZoomOut()
    {
        LeanTween.moveY(gameObject, Camera.main.transform.position.y - 0.6f, 1);
        LeanTween.value(gameObject, animateOrthographicSize, 4.42f, 5, 1);

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

	public void StartCameraZoom()
	{
		Reset();

		//Horrible assumption: We only start zooming when
		//a game begins, so it's safe to assume that we can
		//set gamerunning to true. Don't hate us we're tired
		//and out of red bull
		_gameRunning = true;

        StartCoroutine(CameraZoomIn());
	}

    public void Tracking(GameObject person)
    {
        Target.transform.position = new Vector3(person.transform.position.x, MW.X_StartPosition.transform.position.y, Target.transform.position.z);
    }

    public void Tracking()
    {
        Target.transform.position += Vector3.right * 2.0f * Time.deltaTime;
    }
}
