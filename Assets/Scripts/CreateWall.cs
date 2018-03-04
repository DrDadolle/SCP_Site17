using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWall : MonoBehaviour {

	//The instance for access purposes
	public static CreateWall Instance;

	bool creatingWall;

	GameObject lastPole;

	public GameObject polePrefab;
	public GameObject fencePrefab;


	//Awake
	void Awake(){
		Instance = this;
	}

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

		// Set the position
		Vector3 startPos = pointer.getWorldPoint();
		startPos = pointer.snapPosition (startPos);
		creatingWall = true;

		//
		GameObject startPole = Instantiate (polePrefab, startPos, Quaternion.identity);
		startPole.transform.position = new Vector3 (startPos.x, startPos.y + 0.3f, startPos.z);
		lastPole = startPole;

	}

	//Effectively create the wall
	void setWall(){
		creatingWall = false;
	}

	//Show the future position of the wall
	void updateWall(ShowMousePosition pointer){
		Vector3 currentPosition = pointer.getWorldPoint ();
		currentPosition = pointer.snapPosition (currentPosition);
		currentPosition = new Vector3 (currentPosition.x, currentPosition.y + 0.3f, currentPosition.z);

		if (!currentPosition.Equals (lastPole.transform.position)) {
			createWallSegment (currentPosition);
		}
	}

	//Create effectively a Segment of the Wall
	void createWallSegment(Vector3 current){
		GameObject newPole = Instantiate (polePrefab, current, Quaternion.identity);
		Vector3 middle = Vector3.Lerp (newPole.transform.position, lastPole.transform.position, 0.5f);
		GameObject newFence = Instantiate (fencePrefab, middle, Quaternion.identity);
		newFence.transform.LookAt (lastPole.transform);

		lastPole = newPole;
	}
}
