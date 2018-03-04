using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWall : MonoBehaviour {

	//The instance for access purposes
	public static CreateWall Instance;

	//Indicate if currently build a wall of not
	bool creatingWall;

	Vector3 startPosition;


	private Vector3 it;
	private Vector3 lastIteration;

	//Show the starting point of the building
	private GameObject startPole;

	// Array of currenly created gameObject
	private GameObject[] standbyGO;

	//Public game objects models
	public GameObject polePrefab;
	public GameObject wallPrefab;




	//Awake
	void Awake(){
		Instance = this;
	}


	//Main method to create walls
	public void creationOfWalls(ShowMousePosition pointer){

		if (Input.GetMouseButtonDown (0)) {
			Instance.startWall(pointer);
		}else if(Input.GetMouseButtonUp(0)){
			Instance.setWall();
		}

		if(Instance.creatingWall){
			Instance.updateWall(pointer);
			
		}
	}



	//Initiate the wall segment creation
	void startWall(ShowMousePosition pointer){

		// Set the starting Position
		Vector3 startPos = pointer.getWorldPoint();
		startPosition = pointer.snapPosition (startPos);
		creatingWall = true;

		// Show the starting position with a gameObject
		startPole = Instantiate (polePrefab, startPosition, Quaternion.identity);
		startPole.transform.position = new Vector3 (startPosition.x, startPosition.y, startPosition.z);
	}

	//Effectively create the wall
	void setWall(){
		Destroy (startPole);
		creatingWall = false;
	}

	//Show the future position of the wall
	void updateWall(ShowMousePosition pointer){

		//Get the current mouse position 
		Vector3 currentPosition = pointer.getWorldPoint ();
		currentPosition = pointer.snapPosition (currentPosition);
		currentPosition = new Vector3 (currentPosition.x, currentPosition.y, currentPosition.z);


		if (!currentPosition.Equals (startPosition)) {
			recursiveWallCreation(startPosition, currentPosition);
		}

	}

	//Create effectively a Segment of the Wall
	void createWallSegment(int iteration, bool createOnX, bool isPositive){

		if (!isPositive)
			iteration = -iteration;

		if (createOnX) {
			it = new Vector3 (startPosition.x + iteration, startPosition.y, startPosition.z);
		} else {
			it = new Vector3 (startPosition.x, startPosition.y, startPosition.z + iteration);
		}


		//Get the middle between the 2 last points
		Vector3 middle = Vector3.Lerp (lastIteration, it, 0.5f);
		GameObject newWall = Instantiate (wallPrefab, middle, Quaternion.identity);

		// Rotate the model so the forward axis points in the right direction
		Vector3 YRotation = new Vector3(0, 90, 0);

		newWall.transform.LookAt (startPole.transform);
		newWall.transform.Rotate (YRotation);

		//Update the last iteration
		lastIteration = it;

	}

	/*
	 * StartPos and finishPos are both snapped already
	 * 
	 */
	void recursiveWallCreation(Vector3 startPos, Vector3 finishPos)
	{
		Debug.Log ("AAAAAA");
		int _max_x = Mathf.RoundToInt (finishPos.x - startPos.x);
		int max_x = Mathf.Abs (_max_x); 
		int _max_z = Mathf.RoundToInt (finishPos.z - startPos.z);
		int max_z = Mathf.Abs (_max_z); 

		Debug.Log ("BBBBB");

		//if it is an x-building
		//Default case
		if (max_x != 0 && max_x >= max_z) {
			lastIteration = startPos;
			Debug.Log ("Entering the loop");
			for (int i = 1; i <= max_x; i++) {
				Debug.Log ("Inside the loop : " + i);
				Instance.createWallSegment (i, true, _max_x > 0);
			}
		}
		//else if its an y-building
		else if(max_z != 0) {
			lastIteration = startPos;
			for (int i = 1; i <= max_z; i++) {
				Instance.createWallSegment (i, false, _max_z > 0);
			}
		}

	}






}
