using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.tag);
		if(other.tag == "Finish")
			Debug.Log("Lose");
		else if(other.tag == "Center")
		{
			StartCoroutine(other.GetComponent<ArcController>().Shrink(1,10));
		}
			Debug.Log("StartShrink");
	}
}
