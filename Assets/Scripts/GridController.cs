using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour {
	//do I even need this script??
	//yes I will need to update this for final and challenge by choice stuffs

	GameControllerScript MyGameController;
	public int myGridX, myGridY;

	// Use this for initialization
	void Start () {
		MyGameController = GameObject.Find ("GameController").GetComponent<GameControllerScript> ();
	}

	void OnMouseDown () {
		MyGameController.ProcessGridClick (myGridX, myGridY);

	}

	void OnMouseEnter () {
		MyGameController.EnlargeGridBlock (gameObject);
	}

	void OnMouseExit (){
		MyGameController.ShrinkGridBlock (gameObject);
	}

	void Update() {
		
	}
}
