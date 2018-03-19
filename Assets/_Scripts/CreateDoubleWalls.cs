using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * 
 * 
 */
public class CreateDoubleWalls : MonoBehaviour, IBuildingMethod {

	// First instace of createWall
	private CreateWall InstanceX;
	private CreateWall InstanceZ;

	//contains projected value on the Z axis (keep only X value)
	private Vector3 onX;
	//contains projected value on the X axis (keep only Z value)
	private Vector3 onZ;

	//Instance of the class
	public static CreateDoubleWalls Instance;

	//On Awake
	void Awake(){
		Instance = this;
	}

	// ============================== Implement IBuildingMethod ==============================
	// These methods are called by the MouseController

	// OnLeftButtonPress
	// Used for placing object or initiating drag and dropping
	public void OnLeftButtonPress(MouseController pointer)
	{
		//When clicking for the first time, Sets the starting pole and all the variables
		InstanceX.StartWall(pointer);
		InstanceZ.StartWall(pointer);

	}

	// OnLeftButtonReleaseDuringDragAndDrop
	// Used to finish a drag and drop of the left mouse button
	public void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer)
	{
		//Create the effective walls
		InstanceX.SetWall();
		InstanceZ.SetWall();
	}

	// DuringDragAnDrop
	// Method called during each frame of a drag and drop
	public void DuringDragAndDrop(MouseController pointer)
	{
		onX = ProjectOnAxis (InstanceX.GetCurrentMousePosition (pointer), 1, InstanceX);
		onZ = ProjectOnAxis (InstanceZ.GetCurrentMousePosition (pointer), 2, InstanceZ);


		//Update the walls according to the mouse position
		Debug.Log (onX);
		InstanceX.UpdateWall(onX);
		InstanceZ.UpdateWall(onZ);
	}

	// OnRightButtonPress
	// Used for canceling stuff
	public void OnRightButtonPressDuringDragAndDrop(MouseController pointer)
	{
		// Remove the starting Pole
		Destroy (InstanceX.startPole);
		Destroy (InstanceZ.startPole);

		// Remove all walls
		InstanceX.ClearAndDestroyAllGO();
		InstanceZ.ClearAndDestroyAllGO();
	}

	/**
	 * Project the position on the Z (projectionAxis = 1, keeping X value) or the X (projectionAxis = 2, keeping Z value) axis
	 */
	private Vector3 ProjectOnAxis(Vector3 pos, int projectionAxis, CreateWall instance){
		Vector3 projectedPos = new Vector3 (pos.x, pos.y, pos.z);

		if (projectionAxis.Equals(1)) {
			projectedPos.z = instance.startPosition.z;
		} else if (projectionAxis.Equals(2)) {
			projectedPos.x = instance.startPosition.x;
		}
		return projectedPos;
	}

}
