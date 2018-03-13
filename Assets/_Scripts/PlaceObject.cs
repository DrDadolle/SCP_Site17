using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour {

	//The instance for access purposes
	public static PlaceObject Instance;

	public GameObject objectPrefab;


	//Awake
	void Awake(){
		Instance = this;
	}


	public void placementOfObject(ShowMousePosition pointer){
		if (Input.GetMouseButtonUp (0)) {
			Instance.setObject (pointer);
		}
	}

	//Initiate the object creation
	void setObject(ShowMousePosition pointer){
		// Set the position
		Vector3 pos = pointer.getWorldPoint();
		pos = pointer.snapCenterPosition (pos);

		GameObject objectToPlace = Instantiate(objectPrefab, pos, Quaternion.identity);
		objectToPlace.transform.position = new Vector3 (pos.x, pos.y, pos.z);
	}
}

