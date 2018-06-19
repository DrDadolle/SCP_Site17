using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildWallWithDoor : MonoBehaviour, IBuildingMethod
{
    // Tilemap containing the world
    public Tilemap map;

    // Instance for access purpose
    public static BuildWallWithDoor Instance;

    // Store the ref to the mouse pointer
    public MousePointer pointer;

    // Reference to the Walltile
    //It is empty
    public WallWithDoorTile tile;

    //The sprite dict
    public DictionnaryVariable dictionnaryOfOldTiles;

    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void DuringDragAndDrop(TileBase tile)
    {
        //Do Nothing
    }

    public void OnLeftButtonPress(TileBase tile)
    {
        UpdateTile();
    }

    public void OnLeftButtonReleaseDuringDragAndDrop(TileBase tile)
    {
        //Do Nothing
    }

    public void OnRightButtonPressDuringDragAndDrop(TileBase tile)
    {
        //Do Nothing
    }

    public void OnKeyboardPress(TileBase tile)
    {
        //Do Nothing
    }

    public void OnUpdateWhenTileIsChanged(TileBase tile)
    {
        // TODO : Show Preview
    }

    // =========================== End Implement IBuildingMethod ==============================


    private void UpdateTile()
    {
        Vector3Int tilePos = map.WorldToCell(pointer.GetWorldPoint());


        // Construct a wallwith Door only
        // if there are two opposite neighbours which are walls
        string composition = CommonMethodWall.ReturnCompositionOfNearWalls(tilePos, map);

        if (!(composition.Equals("WWEE") || composition.Equals("EEWW")))
        {
            Debug.Log("Can't build a wall with door here ! Please  make this apparent for the player (tooltip, red cube, etc)");
            return;
        }

        //If we are over a floorTile, keep the old sprite
        if ((map.GetTile(tilePos) is FloorTile))
            AddFloorTileToDict(tilePos, ((FloorTile)map.GetTile(tilePos)).name);
        else
            AddFloorTileToDict(tilePos, "Empty");

        /**
        * JOBS !
        * We should create all the jobs only on mousebutton release !
        * We should create a tmp list of jobs !
        */
        Job j_tmp = new Job(tilePos, () =>
        {
            // Display the building hint on top of this tile position
            map.SetTile(tilePos, tile);
            //FIXME : Maybe indicates that a job is going on
        }, "WallWithDoor", "Building");

        JobManager.jobQueue.Enqueue(j_tmp);
    }

    private void AddFloorTileToDict(Vector3Int v, string s)
    {
        //Save old sprite
        Vector3 v3 = new Vector3(v.x, v.y, v.z);
        // If it contains already this tile
        if (!dictionnaryOfOldTiles.AddToDict(v3, s))
        {
            Debug.LogError("Trying to add twice the old tile data. Return.");
            return;
        }
    }
}
