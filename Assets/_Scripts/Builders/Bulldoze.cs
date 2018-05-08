using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bulldoze : MonoBehaviour, IBuildingMethod{

    // tilemap
    public Tilemap map;

    // Store the ref to the mouse pointer
    public MousePointer pointer;

    // for access purpose
    public static Bulldoze Instance;

    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void OnLeftButtonPress(TileBase tile)
    {
        BulldozeTile();
    }

    public void OnLeftButtonReleaseDuringDragAndDrop(TileBase tile)
    {
        // Do nothing
    }

    public void DuringDragAndDrop(TileBase tile)
    {
        // Do nothing
    }

    public void OnRightButtonPressDuringDragAndDrop(TileBase tile)
    {
        // Do nothing
    }

    public void OnKeyboardPress(TileBase tile)
    {
        // Do nothing
    }

    public void OnUpdateWhenTileIsChanged(TileBase tile)
    {
        // Do nothing
    }

    // ========================== End Implement IBulldozeMethod ==============================

    /**
    *  set the selected tile to null
    */
    private void BulldozeTile()
    {
        Vector3 pos = pointer.GetWorldPoint();
        Vector3Int tilePos = map.WorldToCell(pos);
            

        map.SetTile(tilePos, ResourcesLoading.FloorTilesDictionnary[ResourcesLoading.FloorTilesName.Empty_Tile]);
    }

}
