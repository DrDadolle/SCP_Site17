using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildWall : MonoBehaviour, IBuildingMethod {

    // Tilemap containing the world
    public Tilemap map;

    // Instance for access purpose
    public static BuildWall Instance;

    // Reference to the Walltile
    //It is empty
    public WallTile tile;

    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void DuringDragAndDrop(MouseController pointer)
    {
        UpdateTile(pointer);
    }

    public void OnLeftButtonPress(MouseController pointer)
    {
        UpdateTile(pointer);
    }

    // =========================== End Implement IBuildingMethod ==============================

    private void UpdateTile(MouseController pointer)
    {
        Vector3 pos = pointer.getWorldPoint();
        Vector3Int tilePos = map.WorldToCell(pos);
        //FIXEME it is overriding everything there was before
        map.SetTile(tilePos, tile);
    }
}
