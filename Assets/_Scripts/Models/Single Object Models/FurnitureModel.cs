using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnitureModel {

    // SUPER IMPORTANT TO IDENTIFY THE TYPE !
    public string typeOfFurniture;

    // Position of the tile
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int z;

    // Rotation
    public float rotationOfTheFurniture;

    //The tile type
    public ResourcesLoading.TileBasesName tileItWasPutOn;

    //Hp of the furniture
    public float hp
    {
        get; protected set;
    }

    //Cost of the furniture
    public float cost
    {
        get; protected set;
    }

    // Boolean for placing
    public bool isPending;
    public bool isPreview;

    //Public constructor using InGame data
    public FurnitureModel(Vector3Int pos, string NameOfFurniture, float rot, ResourcesLoading.TileBasesName tileName, bool preview, bool pending)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.typeOfFurniture = NameOfFurniture;
        this.rotationOfTheFurniture = rot;
        this.tileItWasPutOn = tileName;
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
        if (!(o is FurnitureModel))
            return false;
        else
            return this.GetTilePos().Equals(((FurnitureModel)o).GetTilePos());
    }

    public override int GetHashCode()
    {
        return 1465378693 + EqualityComparer<Vector3Int>.Default.GetHashCode(GetTilePos());
    }

}
