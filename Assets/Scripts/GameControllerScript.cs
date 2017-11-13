using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

	public GameObject VehiclePreFab, cubePrefab;
	GameObject Airplane, Train, Boat, gridCube;
	public bool moveAllowed;
	Vector3 airplaneSpawn, trainSpawn, boatSpawn, deliveryDepot;
	Vector3 [,] gridPosition;
	//int airplaneMaxCargo, airplaneCurrentCargo, trainMaxCargo, trainCurrentCargo, boatMaxCargo, boatCurrentCargo;
	int cargoGain;
	//vehicle x and y relative to grid array x and y values
	int airplaneGridX, airplaneGridY, trainGridX, trainGridY, boatGridX, boatGridY;
	int maxX, maxY, gridSize;
	public static GameObject activeVehicle;

	//other variables
	float turnTime, turnSpeed, airplaneSpeed, trainSpeed, boatSpeed, activeVehicleTurnTime, activeVehicleTurnSpeed;
	public int score;
	public Text displayScore, displayAirplaneCargo, displayTrainCargo, displayBoatCargo, timerText;
	public Vector3 targetLocation;
	float seconds, minutes, hours;

	VehicleController ActiveVehicleController;


	// Use this for initialization

	void Start () {

		SetTimerText ();
		turnTime = 1.5f;
		turnSpeed = 1.5f;
		maxX = 16;
		maxY = 9;
		gridPosition = new Vector3[maxX, maxY];
		deliveryDepot = new Vector3 (15, 0);
		cargoGain = 10;
		moveAllowed = false;
		activeVehicle = null;

		for (int y = 0; y < maxY; y++) {
			for (int x = 0; x < maxX; x++) {

				gridPosition [x,y] = new Vector3 (x * 2 - 15, y * 2 - 8, 0);
				gridCube = Instantiate (cubePrefab, gridPosition [x,y], Quaternion.identity);
				gridCube.GetComponent<GridController> ().myGridX = x;
				gridCube.GetComponent<GridController> ().myGridY = y;

				if (deliveryDepot == new Vector3 (x,y)) {
					gridCube.GetComponent<Renderer> ().material.color = Color.black;
				}
			}
		}

		deliveryDepot = gridPosition [15,0];
		SetUpAirplane ();
		SetUpTrain ();
		SetUpBoat ();

	}

	void SetUpAirplane (){
		//Airplane Set Up
		airplaneSpeed = turnSpeed;
		airplaneGridX = 0;
		airplaneGridY = 8;
		airplaneSpawn = gridPosition [airplaneGridX, airplaneGridY];
		Airplane = Instantiate (VehiclePreFab, airplaneSpawn, Quaternion.identity);
		Airplane.GetComponent<Renderer> ().material.color = Color.red;
		Airplane.GetComponent<VehicleController> ().myGridX= airplaneGridX;
		Airplane.GetComponent<VehicleController> ().myGridY = airplaneGridY;
		Airplane.GetComponent<VehicleController> ().mySpawn = airplaneSpawn;
		Airplane.GetComponent<VehicleController> ().myLocation = airplaneSpawn;
		Airplane.GetComponent<VehicleController> ().myCargo = 0;
		Airplane.GetComponent<VehicleController> ().myMaxCargo = 90;
	}

	void SetUpTrain () {
		//Train Set Up
		trainSpeed = turnSpeed * 2;
		trainGridX = 0;
		trainGridY = 0;
		trainSpawn = gridPosition [trainGridX, trainGridY];
		Train = Instantiate (VehiclePreFab, trainSpawn, Quaternion.identity);
		Train.GetComponent<Renderer> ().material.color = Color.green;
		Train.GetComponent<VehicleController> ().myGridX = trainGridX;
		Train.GetComponent<VehicleController> ().myGridY = trainGridY;
		Train.GetComponent<VehicleController> ().mySpawn = trainSpawn;
		Train.GetComponent<VehicleController> ().myLocation = trainSpawn;
		Train.GetComponent<VehicleController> ().myMaxCargo = 200;
		Train.GetComponent<VehicleController> ().myCargo = 0;
	}

	void SetUpBoat () {
		//Boat Set Up
		boatSpeed = turnSpeed * 3;
		boatGridX = 15;
		boatGridY = 8;
		boatSpawn = gridPosition [boatGridX, boatGridY];
		Boat = Instantiate (VehiclePreFab, boatSpawn, Quaternion.identity);
		Boat.GetComponent<Renderer> ().material.color = Color.blue;
		Boat.GetComponent<VehicleController> ().myGridX = boatGridX;
		Boat.GetComponent<VehicleController> ().myGridY = boatGridY;
		Boat.GetComponent<VehicleController> ().mySpawn = boatSpawn;
		Boat.GetComponent<VehicleController> ().myLocation = boatSpawn;
		Boat.GetComponent<VehicleController> ().myMaxCargo = 550;
		Boat.GetComponent<VehicleController> ().myCargo = 0;
	}

	void SetTimerText (){
		timerText.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
	}

	public void ProcessGridClick (int x, int y){
		if (activeVehicle != null) {
			moveAllowed = true;
		} else {
			moveAllowed = false;
		}
		targetLocation = new Vector3 (x, y);

	}

	public void EnlargeGridBlock (GameObject thisCube) {
		if (activeVehicle != null) {
			thisCube.transform.localScale *= 1.5f;
		}
	}

	public void ShrinkGridBlock (GameObject thisCube) {
		if (activeVehicle != null) {
			thisCube.transform.localScale /= 1.5f;
		}
	}

	public void ProcessVehicleClick (GameObject clickedVehicle){
		if (activeVehicle == null) {
			if (clickedVehicle == Airplane) {
				activeVehicle = Airplane;
				activeVehicleTurnSpeed = airplaneSpeed;
			}
			if (clickedVehicle == Boat) {
				activeVehicle = Boat;
				activeVehicleTurnSpeed = boatSpeed;
			}
			if (clickedVehicle == Train) {
				activeVehicle = Train;
				activeVehicleTurnSpeed = trainSpeed;
			}
			activeVehicle.transform.localScale *= 1.2f;
			activeVehicleTurnTime = turnTime + activeVehicleTurnSpeed;
		} else if (clickedVehicle == activeVehicle) {
			activeVehicle.transform.localScale /= 1.2f;
			activeVehicle = null;
			moveAllowed = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Time.time > turnTime) {
			turnTime += turnSpeed;
			Airplane.GetComponent<VehicleController>().UpdateCargo (deliveryDepot, cargoGain);
			Train.GetComponent<VehicleController>().UpdateCargo (deliveryDepot, cargoGain);
			Boat.GetComponent<VehicleController>().UpdateCargo (deliveryDepot, cargoGain);
			if (moveAllowed && Time.time > activeVehicleTurnTime) {
				activeVehicle.GetComponent<VehicleController>().MoveVehicle (targetLocation, gridPosition);
				activeVehicleTurnTime += activeVehicleTurnSpeed;
			}
		}
		SetTimerText ();
		seconds = Mathf.FloorToInt(Time.time % 60f);
		minutes = Mathf.FloorToInt((Time.time / 60f) - (hours*60));
		hours = Mathf.FloorToInt(Time.time / 3600f);

		displayAirplaneCargo.text = "Airplane Cargo:" + Airplane.GetComponent<VehicleController>().myCargo;
		displayTrainCargo.text = "Train Cargo:" + Train.GetComponent<VehicleController>().myCargo;
		displayBoatCargo.text = "Boat Cargo:" + Boat.GetComponent<VehicleController>().myCargo;
		displayScore.text = "Score:" + score;
	}
}
