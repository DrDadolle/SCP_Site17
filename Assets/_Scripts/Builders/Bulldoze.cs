using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bulldoze : MonoBehaviour, IBulldozeMethod {

    // tilemap
    public Tilemap map;

    // for access purpose
    public static Bulldoze Instance;

    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBulldozeMethod ==============================
    // These methods are called by the MouseController

    public void OnLeftButtonPress(MouseController pointer)
    {
        BulldozeTile(pointer);
    }

    // ========================== End Implement IBulldozeMethod ==============================

    /**
    *  set the selected tile to null
    */
    private void BulldozeTile(MouseController pointer)
    {
        Vector3 pos = pointer.getWorldPoint();
        Vector3Int tilePos = map.WorldToCell(pos);
            

        map.SetTile(tilePos, ResourcesLoading.FloorTilesDictionnary[ResourcesLoading.FloorTilesName.Empty_Tile]);
    }

}
