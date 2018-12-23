using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchsimulator : MonoBehaviour
{
    private Vector3 lastMousePosition;

    public Touch t;
    private int touchCount;



    private void Update()
    {
        TrackTouch();
    }

    private void TrackTouch()
    {


        touchCount = 0;

        t = new Touch();
        t.fingerId = 11;
        t.position = Input.mousePosition;
        t.deltaTime = Time.deltaTime;
        t.deltaPosition = Input.mousePosition - lastMousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            t.phase = TouchPhase.Began;
            touchCount++;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            t.phase = t.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary;
            lastMousePosition = Input.mousePosition;
            touchCount++;
        }


        if (Input.GetMouseButtonUp(0))
        {
            t.phase = TouchPhase.Ended;
            // t.deltaPosition = Vector3.zero;
        }

            LogTouches();


    }

    void LogTouches()
    {
        //Debug.Log(t.deltaPosition);
    }



}
