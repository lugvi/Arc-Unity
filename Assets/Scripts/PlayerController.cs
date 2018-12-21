using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameManager gm;
	private void Start() {
		gm = GameManager.instance;
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Finish")
		{
			gm.OnGameOver();
		}
		else if(other.tag == "Center")
		{

			// StartCoroutine(other.GetComponent<ArcController>().Shrink(1,10));
			Debug.Log("StartShrink");
		}

		if(other.tag == "Coin")
		{
			gm.AddScore();
			Destroy(other.gameObject);
		}

	}
}
