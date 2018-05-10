using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorModel {

    //Name of the floor to identify it
    public string nameOfFloor;

    // Position of the tile
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int z;

    // Boolean for placing
    public bool isPending;
    public bool isPreview;

    // Constructor
    public FloorModel(Vector3Int pos, string idOfFloor, bool preview, bool pending)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.isPending = pending;
        this.isPreview = preview;
        this.nameOfFloor = idOfFloor;
    }

    /**
     *  Getter of the tile pos
     */
    public Vector3Int GetTilePos()
    {
        return new Vector3Int(x, y, z);
    }

    /**
    *  Setter of the tile pos
    */
    public void SetTilePos(Vector3Int pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
}
