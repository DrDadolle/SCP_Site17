using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour, IPlaceObjectMethod
{

	//The instance for access purposes
	public static PlaceObject Instance;

    //Furniture to be placed
	public GameObject objectPrefab;

    //Keep in memory the last tile
    private Vector3 lastTile;

    //reference to the display object
    private GameObject furnitureToPlace;

    //Base rotation of all the objects
    private Quaternion baseRotation;

    // Counter of the number of existing furniture
    private int countFurnitures = 0;

    //Value of the layer
    public static int VALUE_OF_FURNITURE_LAYER = 12;
    public static int VALUE_OF_GHOST_FURNITURE_LAYER = 11;

    //Awake
    void Awake(){
		Instance = this;
        baseRotation = Quaternion.identity;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void OnLeftButtonPress(MouseController pointer)
    {
        SetObject(pointer);
    }

    public void OnUpdate(MouseController pointer)
    {
        ShowFurniture(pointer);
    }


    public void OnKeyboardPress()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            furnitureToPlace.transform.Rotate(Vector3.up * 90);
            baseRotation = furnitureToPlace.transform.rotation;
        }

    }


    // ============================== End of IBuildingMethods ==============================

    // Show furniture
    void ShowFurniture(MouseController pointer)
    {
        // Set the position
        Vector3 pos = pointer.getWorldPoint();
        pos = pointer.snapCenterPosition(pos);

        //If furniture does not exist create it
        if (furnitureToPlace == null)
        {
            furnitureToPlace = Instantiate(objectPrefab, pos, baseRotation);
            furnitureToPlace.transform.position = new Vector3(pos.x, pos.y, pos.z);
            // Place it as a child in the hierarchy
            furnitureToPlace.transform.parent = Instance.transform;

            //Change Material of the furniture to blue
            UtilitiesMethod.ChangeMaterialOfRecChildGameObject(furnitureToPlace, ResourcesLoading.ghostly_blue);

            lastTile = pos;
        }
        //Compare to the last position
        //If different update position of furniture
        else if (!pos.Equals(lastTile))
        {
            //update position
            furnitureToPlace.transform.position = new Vector3(pos.x, pos.y, pos.z);
            //Update last position
            lastTile = pos;
        }
    }



    //Initiate the object creation
    void SetObject(MouseController pointer){

        // if collision with something
        if (UtilitiesMethod.IsChildGameObjectOfSpecificMaterial(furnitureToPlace, ResourcesLoading.ghostly_red))
        {
            return;
        }

        // Create the new gameobject
        GameObject objectToPlace = Instantiate(objectPrefab, furnitureToPlace.transform.position, furnitureToPlace.transform.rotation);
        // Place it as a child in the hierarchy
        objectToPlace.transform.parent = Instance.transform;

        // Change name to check
        countFurnitures++;
        objectToPlace.name += "_" + countFurnitures; 

        //Set the trigger
        Collider tmp_collider = objectToPlace.GetComponent<Collider>();
        tmp_collider.isTrigger = false;

        //Changing its layer from GhostFurniture (11) to Furniture (12)
        objectToPlace.layer = VALUE_OF_FURNITURE_LAYER;
        Rigidbody tmp_rigid = objectToPlace.AddComponent<Rigidbody>();
        tmp_rigid.isKinematic = true;

        //Remove the behaviour script
        Destroy(objectToPlace.GetComponent<FurnitureBehaviour>());
        

        //Destroy the furniture to display
        Destroy(furnitureToPlace);
    }

    //Destroy the leftover furniture
    public static void DestroyLeftovertGhostlyFurniture()
    {
        if (Instance.furnitureToPlace != null && !ConstructionController.IsPlacingFurnitures())
        {
            Destroy(Instance.furnitureToPlace);
        }
    }

}

