using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	GameManager gm;
	private void Start() {
		gm = GameManager.instance;
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Finish")
		{
			gm.OnGameOver();
			GetComponent<ParticleSystem>().Play();
		}
		else if(other.tag == "Center")
		{

			StartCoroutine(other.GetComponent<ArcController>().Shrink());
			Debug.Log("StartShrink");
		}

		if(other.tag == "Coin")
		{
			gm.AddScore();
			Destroy(other.gameObject);
		}

	}
}
