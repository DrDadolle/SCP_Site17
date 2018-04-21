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

    //Reference to the old sprite
    public SpriteVariable oldSpriteOfFloor;

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

    public void OnKeyboardPress()
    {
        //Do Nothing
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

        //If we are over a floorTile, set the old sprite
        if ((map.GetTile(tilePos) is FloorTile))
            oldSpriteOfFloor.sprite = ((FloorTile)map.GetTile(tilePos)).SpriteOfFloor;
        else
            oldSpriteOfFloor.sprite = null;

        //Place the wall
        map.SetTile(tilePos, tile);
    }
}
