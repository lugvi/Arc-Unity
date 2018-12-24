using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class methods : MonoBehaviour {

	// Use this for initialization
	 // private void TrackTouch()
    // {

    //     Touch t = new Touch();
    //     t.fingerId = 11;
    //     t.position = Input.mousePosition;
    //     // t.deltaTime = Time.deltaTime;
    //     if (lastMousePosition != Vector3.zero)
    //         t.deltaPosition = Input.mousePosition - lastMousePosition;
    //     else
    //     {
    //         t.deltaPosition = Vector2.zero;
    //     }

    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         t.phase = TouchPhase.Began;
    //         lastMousePosition = Input.mousePosition;
    //     }
    //     else if (Input.GetMouseButton(0))
    //     {
    //         t.phase = t.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary;
    //         lastMousePosition = Input.mousePosition;
    //     }


    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         t.phase = TouchPhase.Ended;
    //         lastMousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    //         t.deltaPosition = Vector3.zero;
    //     }



    // }


	 // public void TapMovement()
    // {
    //     Vector2 mousepos;



    //     // Debug.Log((Input.mousePosition.x - Screen.width / 2) + " " + (Input.mousePosition.y - Screen.height / 2));
    //     if (Input.touchCount > 0 && !moving && playing)
    //     {
    //         mousepos.x = Input.GetTouch(0).position.x - Screen.width / 2;
    //         mousepos.y = Input.GetTouch(0).position.y - Screen.height / 2;
    //         if (Mathf.Abs(mousepos.x) > Mathf.Abs(mousepos.y))
    //         {
    //             if (mousepos.x > 0)
    //             {
    //                 destination.x = playerpos.x + 1;
    //                 StartCoroutine(MovePlayer(destination));
    //                 moveGrid(Vector2Int.right);
    //             }

    //             if (mousepos.x < 0)
    //             {
    //                 destination.x = playerpos.x - 1;
    //                 StartCoroutine(MovePlayer(destination));
    //                 moveGrid(Vector2Int.left);
    //             }

    //         }
    //         else
    //         {
    //             if (mousepos.y > 0)
    //             {
    //                 destination.y = playerpos.y + 1;
    //                 StartCoroutine(MovePlayer(destination));
    //                 moveGrid(Vector2Int.up);
    //             }

    //             if (mousepos.y < 0)
    //             {
    //                 destination.y = playerpos.y - 1;
    //                 StartCoroutine(MovePlayer(destination));
    //                 moveGrid(Vector2Int.down);
    //             }

    //         }
    //     }
    //}
}
