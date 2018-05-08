using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceFurnitures : MonoBehaviour, IBuildingMethod
{

    //Reference to the tilemap
    public Tilemap map;

    // Store the ref to the mouse pointer
    public MousePointer pointer;

    // for access purpose
    public static PlaceFurnitures Instance;

    // Ref to the rotation float
    public FloatVariable rotation;

    //The sprite dict
    public DictionnaryVariable dictionnaryOfOldTiles;

    private List<Job> listOfPotentialJobs = new List<Job>();

    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void OnKeyboardPress()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rotation.thefloat += 90;
            rotation.thefloat = rotation.thefloat % 360;
        }
    }

    public void OnLeftButtonPress(TileBase tile)
    {
        UpdateTile(tile as FurnitureTile);
    }

    public void OnLeftButtonReleaseDuringDragAndDrop(TileBase tile)
    {
        //Do nothing
    }

    public void DuringDragAndDrop(TileBase tile)
    {
        //Do nothing
    }

    public void OnRightButtonPressDuringDragAndDrop(TileBase tile)
    {
        //Do nothing
    }

    // ============================== Implement IPlaceObjectMethod ==============================

    private void UpdateTile(FurnitureTile tile)
    {
        Vector3Int tilePos = map.WorldToCell(pointer.GetWorldPoint());

        // Check if we can put this furniture there
        //If we are not over a valid floorTile, bail out
        if (!tile.furnitureData.tilesItCanBePlacedOn.Contains((FloorTile)map.GetTile(tilePos)))
        {
            // Bail out
            Debug.Log("Can't place " + tile.furnitureData.furniturePrefab.name + " on " + map.GetTile(tilePos).name + " tile !");
            return;
        }

        //Save old sprite
        Vector3 v3 = new Vector3(tilePos.x, tilePos.y, tilePos.z);
        // If it contains already this tile
        if (!dictionnaryOfOldTiles.AddToDict(v3, ((FloorTile)map.GetTile(tilePos)).name))
        {
            Debug.LogError("Trying to add twice the old tile data. Return.");
            return;
        }

        /**
        * JOBS !
        * We should create all the jobs only on mousebutton release !
        * We should create a tmp list of jobs !
        */
        Job j_tmp = new Job(tilePos, (theJob) =>
        {
            //Set the furniture tile on the map
            map.SetTile(tilePos, tile);
            //FIXME : Maybe indicates that a job is going on
        });
        JobManager.jobQueue.Enqueue(j_tmp);
    }
}
