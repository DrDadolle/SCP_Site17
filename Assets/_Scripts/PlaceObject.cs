using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour, IBuildingMethod {

	//The instance for access purposes
	public static PlaceObject Instance;

	public GameObject objectPrefab;


	//Awake
	void Awake(){
		Instance = this;
	}

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void OnLeftButtonPress(MouseController pointer)
    {
        Instance.SetObject(pointer);
    }

    public void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer)
    {
        //Do Nothing
    }

    public void DuringDragAndDrop(MouseController pointer)
    {
        //Do Nothing
    }

    public void OnRightButtonPressDuringDragAndDrop(MouseController pointer)
    {
        //Do Nothing
    }

    // ============================== End of IBuildingMethods ==============================

	//Initiate the object creation
	void SetObject(MouseController pointer){
		// Set the position
		Vector3 pos = pointer.getWorldPoint();
		pos = pointer.snapCenterPosition (pos);

		GameObject objectToPlace = Instantiate(objectPrefab, pos, Quaternion.identity);
		objectToPlace.transform.position = new Vector3 (pos.x, pos.y, pos.z);
	}

}

