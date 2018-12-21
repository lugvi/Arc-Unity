using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcController : MonoBehaviour
{
    public GameObject wing1;

    public GameObject wing2;
    public GameObject coin;

    public Direction rotationDirection;

    public Color col;
   

    public float rotationSpeed;

    private void Start() {
        wing1.GetComponent<SpriteRenderer>().color = col;
        wing2.GetComponent<SpriteRenderer>().color = col;
    }
    private void OnEnable() {
        wing1.GetComponent<SpriteRenderer>().color = col;
        wing2.GetComponent<SpriteRenderer>().color = col;
    }

    public void Rotate()
    {
         transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed * ((float)rotationDirection-0.5f)*2);
    }

    // private void OnGUI()
    // {
    //     if (GUI.Button(new Rect(0, 0, 200, 200), "shrink"))
    //         StartCoroutine(Shrink(1));
    // }

    public IEnumerator Shrink(float angle,float speed)
    {
        while (wing2.transform.localRotation.z != angle)
        {
            Debug.Log(wing2.transform.localRotation.z);
            wing2.transform.Rotate(Vector3.forward * angle * Time.deltaTime*speed);
            yield return 0;
        }
    }


    // private void OnTriggerEnter2D(Collider2D other) {
    //     Debug.Log("Lose");
    // }

}


public enum Direction
{
    Clockwise,CounterClockwise
}
