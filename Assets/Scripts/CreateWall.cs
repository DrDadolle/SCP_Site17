using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWall : MonoBehaviour {

	//The instance for access purposes
	public static CreateWall Instance;

	//Indicate if currently build a wall of not
	bool dragAndDropping;

	//Store the starting position of the drag and drop
	private Vector3 startPosition;

	private Vector3 it;
	private Vector3 lastIteration;

	/*keep in memory which axis we were building on;
	 * 0 = none
	 * 1 = X
	 * 2 = Z
	 */
	private int buildingAxis = 0;

	//Show the starting point of the wall
	private GameObject startPole;

	//Show the ending point of the wall
	private GameObject endPole;

	// Array of provisiorary wall's GO
	private List<GameObject> standbyGO;

	//Public game objects models
	public GameObject polePrefab;
	public GameObject wallPrefab;



	//Awake
	void Awake(){
		Instance = this;
		//Create a temp WallGameObject list
		standbyGO = new List<GameObject> ();
	}


	//Main method to create walls
	public void creationOfWalls(ShowMousePosition pointer){

		if (Input.GetMouseButtonDown (0)) {
			Instance.startWall(pointer);
		}else if(Input.GetMouseButtonUp(0) && Instance.dragAndDropping){
			Instance.setWall();
		}

		if(Instance.dragAndDropping){
			//If right click is pressed to cancel
			if (Input.GetMouseButtonDown (1)) {
				// Remove the starting Pole
				Destroy (Instance.startPole);

				// Remove all walls
				clearAndDestroyAllGO ();

				// undrag
				Instance.dragAndDropping = false;

			} else {
				Instance.updateWall (pointer);
			}
		}
	}



	//Initiate the wall segment creation
	void startWall(ShowMousePosition pointer){

		// Set the starting Position
		Vector3 startPos = pointer.getWorldPoint();
		startPosition = pointer.snapPosition (startPos);
		dragAndDropping = true;
		lastIteration = startPosition;

		// Show the starting position with a gameObject
		startPole = Instantiate (polePrefab, startPosition, Quaternion.identity);
		startPole.transform.position = new Vector3 (startPosition.x, startPosition.y, startPosition.z);
	}

	//Effectively create the wall
	void setWall(){

		// Show the finishing position with a gameObject
		endPole = Instantiate (polePrefab, lastIteration, Quaternion.identity);
		endPole.transform.position = new Vector3 (lastIteration.x, lastIteration.y, lastIteration.z);

		if (lastIteration.Equals (startPosition)) {
			Destroy (Instance.startPole);
			Destroy (Instance.endPole);
		}

		//Clear the previous arraylist
		standbyGO.Clear();

		//Finish Drag And drop
		dragAndDropping = false;
	}


	//Show the future position of the wall
	void updateWall(ShowMousePosition pointer){

		//Get the current mouse position 
		Vector3 currentPosition = pointer.getWorldPoint ();
		currentPosition = pointer.snapPosition (currentPosition);
		currentPosition = new Vector3 (currentPosition.x, currentPosition.y, currentPosition.z);

		if (!currentPosition.Equals(lastIteration)) {
			recursiveWallBuilder(startPosition, currentPosition);
		}
	}




	/**
	 * Create recursively walls between a start position and an end position
	 * StartPos and finishPos are both snapped already
	 * 
	 */
	void recursiveWallBuilder(Vector3 startPos, Vector3 finishPos)
	{
		int _max_x = Mathf.RoundToInt (finishPos.x - startPos.x);
		int max_x = Mathf.Abs (_max_x); 
		int _max_z = Mathf.RoundToInt (finishPos.z - startPos.z);
		int max_z = Mathf.Abs (_max_z); 

			//if it is an x-axis building
			//Default case
		if (max_x >= max_z) {
			if (max_x > standbyGO.Count) {
				lastIteration = startPos;
				for (int i = 0; i < max_x; i++) {
					buildingAxis = 1;
					Instance.createWallSegment (i + 1, _max_x > 0);
				}
			} else {
				destroyObsoleteGO (_max_x);
		
			}
		}
		//else if its an z-axis building
		else {
			if (max_z > standbyGO.Count) {
				lastIteration = startPos;
				for (int i = 0; i < max_z; i++) {
					buildingAxis = 2;
					Instance.createWallSegment (i + 1, _max_z > 0);
				}
			} else {
				destroyObsoleteGO (_max_z);
			}
		}
	}

	//==========================================
	//Create effectively a Segment of the Wall
	void createWallSegment(int iteration, bool isPositive){

		if (!isPositive)
			iteration = -iteration;

		//Check that all the object are build on the same axis (x or z one)
		checkAllObjectOnXorZAxis (iteration);

		//Check that all the object are build on the same side (negative or positive one)
		checkAllObjectPositiveOrNegativeAxis ();

		// checks if it is a new item
		if (Mathf.Abs(iteration) > standbyGO.Count) {

			//Get the middle between the 2 last points
			Vector3 middle = Vector3.Lerp (lastIteration, it, 0.5f);

			//Create the wall
			GameObject newWall = Instantiate (wallPrefab, middle, Quaternion.identity);

			//Add it to the arraylist
			standbyGO.Add (newWall);

			// Rotate the model so the forward axis points in the right direction
			Vector3 YRotation = new Vector3 (0, 90, 0);
			newWall.transform.LookAt (startPole.transform);
			newWall.transform.Rotate (YRotation);

			//Destroy previous endPole
			/*
			if (endPole == null) {
				endPole = Instantiate (polePrefab, it, Quaternion.identity);
			} else if (!endPole.transform.position.Equals (it)){
				endPole.transform.position = new Vector3 (it.x, it.y, it.z);
			}*/

		}
		//Update the last iteration
		lastIteration = it;
	}
		
	//Check that all the object are build on the same axis (x or z one)
	void checkAllObjectOnXorZAxis(int iteration){
		
		//Create the it vector indicating the new point of the wall
		if (buildingAxis == 1) {
			it = new Vector3 (startPosition.x + iteration, startPosition.y, startPosition.z);
		} else {
			it = new Vector3 (startPosition.x, startPosition.y, startPosition.z + iteration);
		}
			
		// Check if we are still building on the right axis
		if (standbyGO.Count > 0) {
			if (buildingAxis == 1 && standbyGO [0].transform.position.z != startPosition.z) {
				buildingAxis = 2;
				clearAndDestroyAllGO ();
			} else if (buildingAxis == 2 && standbyGO [0].transform.position.x != startPosition.x) {
				buildingAxis = 1;
				clearAndDestroyAllGO ();
			}
		}
	}

	//Check that all the object are build on the same side (negative or positive one)
	void checkAllObjectPositiveOrNegativeAxis(){
		if (standbyGO.Count > 0) {
			if (buildingAxis == 1) {
				if (it.x > startPosition.x && standbyGO [0].transform.position.x < startPosition.x)
					clearAndDestroyAllGO ();
				else if (it.x < startPosition.x && standbyGO [0].transform.position.x > startPosition.x)
					clearAndDestroyAllGO ();
			} else {
				if (it.z > startPosition.z && standbyGO [0].transform.position.z < startPosition.z)
					clearAndDestroyAllGO ();
				else if (it.z < startPosition.z && standbyGO [0].transform.position.z > startPosition.z)
					clearAndDestroyAllGO ();
			}
		}
	}
		
	//Utility Method to empty the gameObject Array
	void clearAndDestroyAllGO()
	{
		standbyGO.ForEach(delegate(GameObject obj) {
			Destroy(obj);
		});
		standbyGO.Clear ();
	}


	//destroy
	void destroyObsoleteGO(int pseudoMax){
		int pseudoMaxAbs = Mathf.Abs (pseudoMax);

		for (int i = pseudoMaxAbs  ; i < standbyGO.Count; i++) {
			if (pseudoMax > 0) {
				if (buildingAxis == 1) {
					lastIteration.x -= 1;
				} else {
					lastIteration.z -= 1;
				}
			} else if (pseudoMax < 0) {
				if (buildingAxis == 1) {
					lastIteration.x += 1;
				} else {
					lastIteration.z += 1;
				}
			} else {
				if (buildingAxis == 1) {
					lastIteration.x = startPosition.x;
				} else {
					lastIteration.z = startPosition.z;
				}
			}

			/*if (endPole != null) 
				endPole.transform.position = lastIteration;
			*/

			Destroy (standbyGO [i]);
			standbyGO.RemoveAt (i);
		}
	}

}
