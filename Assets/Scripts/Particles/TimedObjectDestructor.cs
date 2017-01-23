using UnityEngine;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviour {

    private float timeOut = 2.0f;
    private bool detachChildren = false;

    void Awake()
    {
        Invoke("DestroyNow", timeOut);
    }

    void DestroyNow()
    {
        if (detachChildren)
            transform.DetachChildren();

        DestroyObject(gameObject);
    }
}
