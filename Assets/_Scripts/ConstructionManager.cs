using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour {

	private static MouseMode currentMouseMode;

	public enum MouseMode {
		NotBuilding,
		BuildingWall,
		BuildingObjects
	};


	// Use this for initialization
	void Start () {
		currentMouseMode = MouseMode.NotBuilding;	
	}
	
	// Update is called once per frame
	void Update () {

		//Change the mode of construction
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			currentMouseMode = MouseMode.NotBuilding;
		} else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			currentMouseMode = MouseMode.BuildingWall;
		} else if (Input.GetKeyUp (KeyCode.Alpha3)) {
			currentMouseMode = MouseMode.BuildingObjects;
		}
	}

	public static MouseMode getCurrentMouseMode()
	{
		return currentMouseMode;
	}

    //Sets the building mode to BuildingWall;
    public void Set_Mode_Build_Wall()
    {
        currentMouseMode = MouseMode.BuildingWall;
    }

    //Sets the building mode to BuildingObjects
    public void Set_Mode_Place_Office()
    {
        currentMouseMode = MouseMode.BuildingObjects;
    }

}
