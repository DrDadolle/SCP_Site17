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

            //Change Material of the furniture to blue
            ChangeMaterialOfFurniture(ResourcesLoading.ghostly_blue);

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

    //Utilities method to change the material of all the children
    void ChangeMaterialOfFurniture(Material material)
    {
        Renderer[] children;
        children = furnitureToPlace.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = material;
            }
            rend.materials = mats;
        }
    }

    //Initiate the object creation
    void SetObject(MouseController pointer){

        GameObject objectToPlace = Instantiate(objectPrefab, furnitureToPlace.transform.position, furnitureToPlace.transform.rotation);

        //Destroy the furniture to display
        Destroy(furnitureToPlace);
    }

    //Destroy the leftover furniture
    public static void DestroyLeftovertGhostlyFurniture()
    {
        if (Instance.furnitureToPlace != null && !ConstructionManager.IsPlacingFurnitures())
        {
            Destroy(Instance.furnitureToPlace);
        }
    }

}

