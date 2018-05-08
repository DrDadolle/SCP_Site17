using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComputerModel : FurnitureModel
{
    //Public constructor using InGame data
    public ComputerModel(Vector3Int pos, float rot, FurnitureData data, ResourcesLoading.TileBasesName tileName, bool preview, bool pending) :
                base(pos, data.nameOfFurniture, rot, tileName, preview, pending)
    {
        this.hp = data.maxHealth;
        this.cost = data.costDollar;
    }
}
