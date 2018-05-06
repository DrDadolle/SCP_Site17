using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallWithDoorsModel {

    // Position of the tile
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int z;

    //The tile type
    public ResourcesLoading.TileBasesName tileItWasPutOn;

    //Public constructor using InGame data
    public WallWithDoorsModel(Vector3Int pos, ResourcesLoading.TileBasesName tileName)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.tileItWasPutOn = tileName;
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

    /**
    *  Override Equals method 
    */
    public override bool Equals(object o)
    {
        if (!(o is WallWithDoorsModel))
            return false;
        else
            return this.GetTilePos().Equals(((WallWithDoorsModel)o).GetTilePos());
    }

    public override int GetHashCode()
    {
        return 1465378693 + EqualityComparer<Vector3Int>.Default.GetHashCode(GetTilePos());
    }
}
