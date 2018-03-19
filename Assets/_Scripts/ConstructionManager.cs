using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour {

	private static BuildingMode currentBuildingMode;

	public enum BuildingMode {
		NotBuilding,
		BuildingWall,
		BuildingDoubleWalls,
		BuildingObjects
	};


	// Use this for initialization
	void Start () {
		currentBuildingMode = BuildingMode.NotBuilding;	
	}
	
	// Update is called once per frame
	void Update () {

		//Change the mode of construction
		if (Input.GetKeyUp (KeyCode.Alpha1)) {
			currentBuildingMode = BuildingMode.NotBuilding;
		} else if (Input.GetKeyUp (KeyCode.Alpha2)) {
			currentBuildingMode = BuildingMode.BuildingWall;
		} else if (Input.GetKeyUp (KeyCode.Alpha3)) {
			currentBuildingMode = BuildingMode.BuildingObjects;
		}
	}

	public static BuildingMode getCurrentBuildingMode()
	{
		return currentBuildingMode;
	}

    //Sets the building mode to BuildingWall;
    public void Set_Mode_Build_Wall()
    {
        currentBuildingMode = BuildingMode.BuildingWall;
    }

    //Sets the building mode to BuildingObjects
    public void Set_Mode_Place_Furnitures()
    {
        currentBuildingMode = BuildingMode.BuildingObjects;
    }

	//Sets the building mode to BuildingDoubleWalls
	public void Set_Mode_Build_Double_Walls()
	{
		currentBuildingMode = BuildingMode.BuildingDoubleWalls;
	}

}
