using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallModel {

    // Position of the tile
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int z;

    //Hp of the wall
    public float hp
    {
        get; protected set;
    }

    //Cost of the wall
    public float cost
    {
        get; protected set;
    }

    // Boolean for placing
    public bool isPending;
    public bool isPreview;

    /**
     * Public constructor using InGame data
     */
    public WallModel(Vector3Int pos, WallData data)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.hp = data.maxHealth;
        this.cost = data.costDollar;
        this.isPending = false;
        this.isPreview = false;
    }

    /**
    * Public constructor using InGame data and pending/Preview booleans
    */
    public WallModel(Vector3Int pos, WallData data, bool preview, bool pending)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.hp = data.maxHealth;
        this.cost = data.costDollar;
        this.isPending = pending;
        this.isPreview = preview;
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
