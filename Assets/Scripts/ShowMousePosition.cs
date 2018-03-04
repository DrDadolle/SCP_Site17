using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Method creating basic Wall
 */
public class ShowMousePosition : MonoBehaviour {

	// Last Position of the Mouse
	private Vector3 lastMousePosition;

	//Storing the building function called
	private delegate void BuildingGameObjects(ShowMousePosition pointer);
	BuildingGameObjects buildingMethod; 

	//Reference to the GameObject MousePointer
	public GameObject mousePointer;

	//Last Mouse Position
	ShowMousePosition pointer;

	// Use this for initialization
	void Start () {
		pointer = GetComponent<ShowMousePosition> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ConstructionManager.getCurrentMouseMode () == ConstructionManager.MouseMode.BuildingWall) {
			//Activating the mousePointer
			mousePointer.SetActive(true);
			mousePointer.transform.position = snapPosition (getWorldPoint ());

			//Setting the WallBuilder Method
			buildingMethod = CreateWall.Instance.creationOfWalls;

		} else if (ConstructionManager.getCurrentMouseMode () == ConstructionManager.MouseMode.BuildingObjects) {
			//Activating the mousePointer
			mousePointer.SetActive(true);
			mousePointer.transform.position = snapCenterPosition (getWorldPoint ());

			//Setting the ObjectPlacer Method
			buildingMethod = PlaceObject.Instance.placementOfObject;

		} else {
			mousePointer.SetActive(false);
		}

		//Calling the Building Methods
		if (mousePointer.activeSelf) {
			buildingMethod (pointer);
		}

	}

	public Vector3	getWorldPoint()
	{
		Camera camera = GetComponent<Camera>();
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			lastMousePosition = hit.point;
			return lastMousePosition;
		}
		//out of bound raycast
		return lastMousePosition;
	}

	/*
	 * Snap the position at the edge of the grid
	*/
	public Vector3 snapPosition(Vector3 original){
		Vector3 snapped;
		snapped.x = Mathf.Floor (original.x + 0.5f);
		snapped.y = Mathf.Floor (original.y + 0.5f);
		snapped.z = Mathf.Floor (original.z + 0.5f);
		return snapped;
	}

	/*
	 * Snap the position at the center of the grid
	*/
	public Vector3 snapCenterPosition(Vector3 original){
		Vector3 snapped;
		snapped.x = Mathf.Floor (original.x) + 0.5f;
		snapped.y = Mathf.Floor (original.y + 0.5f);
		snapped.z = Mathf.Floor (original.z) + 0.5f;
		return snapped;
	}
}
