using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            RaycastHit2D hit = Physics2D.Raycast(v3, -Vector2.up);
            if (hit.collider != null)
            {
                if (Camera.main.GetComponent<CameraController>().StartTranslate)
                {
                    var viewer = hit.collider.gameObject.GetComponent<ViewerController>();
                    if (viewer.HatesTrump)
                    {
                        GameObject.Find("GameManager").GetComponent<GameController>().score++;
                        AudioController.Instance.PlayEffect(Effect.UI_TAP);
                        viewer.StandUpThenDown();
                        viewer.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
            }
        }
    }
}
