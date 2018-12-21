using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchsimulator : MonoBehaviour
{
    #region Touch_Fields
    private Vector3 lastMousePosition;
    private Touch[] touches;
    private int touchCount;
    #endregion

    private void Awake()
    {
        touches = new Touch[1];
    }

    private void Update()
    {
        TrackTouches();
        CheckForSwipe();
    }

    // track touches for all platform
    private void TrackTouches()
    {

#if UNITY_EDITOR
        touchCount = 0;
        for (int i = 0; i < touches.Length; i++)
#else
            touchCount = Input.touchCount;
            for (int i= 0; i < Math.Min(touches.Length, Input.touchCount); i++)
#endif
        {
#if UNITY_EDITOR
            Touch t;

            // register mouse as touch
            //-------------------------------------
            t = new Touch();
            t.fingerId = 10 + i;
            t.position = Input.mousePosition;
            t.deltaTime = Time.deltaTime;
            t.deltaPosition = Input.mousePosition - lastMousePosition;

            if (Input.GetMouseButtonDown(i))
            {
                t.phase = TouchPhase.Began;
                touchCount++;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(i))
            {
                t.phase = t.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary;
                lastMousePosition = Input.mousePosition;
                touchCount++;
            }


            if (Input.GetMouseButtonUp(0))
            {

                t.phase = TouchPhase.Ended;
                lastMousePosition = Vector3.zero;
            }
            //---------------------------------------

            touches[i] = t;
#else
                touches[i] = Input.GetTouch(i); // save touches in container
#endif
        }
    }
    int _direction;

    Vector2 lastPosition;

	public	Vector3 cardinalDirection;

    public bool swiping;

    public float moveDragValue;
    private void CheckForSwipe()
    {
        if (touchCount == 0)
        {
            _direction = 0;
			cardinalDirection = Vector2.zero;
            return;
        }

        if (touches[0].deltaPosition.sqrMagnitude != 0)
        {
            if (swiping == false)
            {
                swiping = true;
                return;
            }

            Vector2 direction = touches[0].position - lastPosition;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > moveDragValue)
				{
					cardinalDirection = Vector2.right;
                    Debug.Log("Right");
				}
                else if (direction.x < -moveDragValue)
				{
					cardinalDirection = Vector2.left;

                    Debug.Log("Left");
				}
            }
            else
            {
                if (direction.y > moveDragValue)
				{

					cardinalDirection = Vector2.up;
                    Debug.Log("up");
				}
                else if (direction.y < -moveDragValue)
				{

					cardinalDirection = Vector2.down;
                    Debug.Log("down");
				}
            }

        }
        else
        {
            swiping = false;
        }

        lastPosition = touches[0].position;
    }
}
