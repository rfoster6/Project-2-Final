using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour {
	public int myCargo, myMaxCargo, myGridX, myGridY;
	public Vector3 mySpawn, myLocation;

	GameControllerScript MyGameController;

	// Use this for initialization
	void Start () {
		MyGameController = GameObject.Find ("GameController").GetComponent<GameControllerScript> ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown () {
		MyGameController.ProcessVehicleClick (gameObject);
	}

	public void UpdateCargo (Vector3 deliveryDepot, int cargoGain){
		//game controls about cargo here
		if (myLocation == mySpawn && myCargo < myMaxCargo){
			myCargo += cargoGain;
			if (myCargo > myMaxCargo) {
				myCargo = myMaxCargo;
			}
		}

		if (myLocation == deliveryDepot) {
			MyGameController.score += myCargo;
			myCargo = 0;
		}
	}

	public void MoveVehicle (Vector3 targetLocation, Vector3 [,] gridPosition){
			if (myGridX < targetLocation.x) {
				myGridX++;
			}
			if (myGridX > targetLocation.x) {
				myGridX--;
			}
			if (myGridY < targetLocation.y) {
				myGridY++;
			}
			if (myGridY > targetLocation.y) {
				myGridY--;
			}
			myLocation = gridPosition [myGridX, myGridY];
			gameObject.transform.position = myLocation;
	}


}
