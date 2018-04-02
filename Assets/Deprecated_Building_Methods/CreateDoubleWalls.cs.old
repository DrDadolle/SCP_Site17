using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Method inheriting from CreateWall creating double walls
 * 
 */
public class CreateDoubleWalls : CreateWall, IBuildingMethod {

	//contains projected value on the Z axis (keep only X value)
	private Vector3 onX;
	//contains projected value on the X axis (keep only Z value)
	private Vector3 onZ;

    //Instance of the class
    public static CreateDoubleWalls InstanceDoubleWall;

	//On Awake
	void Awake(){
        InstanceDoubleWall = this;
    }

	// ============================== Implement IBuildingMethod ==============================
	// These methods are called by the MouseController

	// OnLeftButtonPress
	// Used for placing object or initiating drag and dropping
	public override void OnLeftButtonPress(MouseController pointer)
	{
        //When clicking for the first time, Sets the starting pole and all the variables
        Debug.Log("I'm in");
		StartWall(pointer);

	}

	// OnLeftButtonReleaseDuringDragAndDrop
	// Used to finish a drag and drop of the left mouse button
	public override void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer)
	{
		//Create the effective walls
		SetWall();
	}

	// DuringDragAnDrop
	// Method called during each frame of a drag and drop
	public override void DuringDragAndDrop(MouseController pointer)
	{
        Debug.Log("I'm in -- DuringDragAndDrop");
        onX = ProjectOnAxis (GetCurrentMousePosition (pointer), 1);
		onZ = ProjectOnAxis (GetCurrentMousePosition (pointer), 2);


		//Update the walls according to the mouse position
		UpdateWall(GetCurrentMousePosition(pointer));
		//UpdateWall(onZ);
    }

	// OnRightButtonPress
	// Used for canceling stuff
	public override void OnRightButtonPressDuringDragAndDrop(MouseController pointer)
	{
		// Remove the starting Pole
		Destroy (startPole);

		// Remove all walls
		ClearAndDestroyAllGO();
	}

    // ============================== End of IBuildingMethods ==============================

    /**
	 * Project the position on the Z (projectionAxis = 1, keeping X value) or the X (projectionAxis = 2, keeping Z value) axis
	 */
    private Vector3 ProjectOnAxis(Vector3 pos, int projectionAxis){
		Vector3 projectedPos = new Vector3 (pos.x, pos.y, pos.z);

		if (projectionAxis.Equals(1)) {
			projectedPos.z = startPosition.z;
		} else if (projectionAxis.Equals(2)) {
			projectedPos.x = startPosition.x;
		}
		return projectedPos;
	}

}
