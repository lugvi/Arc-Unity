﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcController : MonoBehaviour
{
    public GameObject wing1;

    public GameObject wing2;
    public GameObject coin;

    public Direction rotationDirection;

    public float ArcCloseTime;

    public Color col;


    public float rotationSpeed;

    GameManager gm;

    private void Start()
    {   
        gm = GameManager.instance;
        ArcCloseTime = gm.arcCloseTime;
        wing1.GetComponent<SpriteRenderer>().color = col;
        wing2.GetComponent<SpriteRenderer>().color = col;
    }
    private void OnEnable()
    {
        wing1.GetComponent<SpriteRenderer>().color = col;
        wing2.GetComponent<SpriteRenderer>().color = col;
    }

    public void Rotate()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed * ((float)rotationDirection - 0.5f) * 2);
    }

    // private void OnGUI()
    // {
    //     if (GUI.Button(new Rect(0, 0, 200, 200), "shrink"))
    //         StartCoroutine(Shrink(1));
    // }

    public IEnumerator Shrink()
    {
        float z = wing1.transform.localRotation.eulerAngles.z;

        //Debug.Log(wing1.transform.localRotation.eulerAngles.z);
        //float starttime = Time.time;
        while (wing2.transform.localRotation.eulerAngles.z > 180 + z && gameObject.activeInHierarchy)
        {
            if(!gm.playing)
                break;
        //Debug.Log(wing2.transform.localRotation.eulerAngles.z + " " +z);
            wing2.transform.Rotate(Vector3.back * Time.deltaTime * ((180-z)/ArcCloseTime));
            yield return null;
        }

        //Debug.Log(Time.time-starttime);
    }


    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("Lose");
    // }

}


public enum Direction
{
    Clockwise, CounterClockwise
}
