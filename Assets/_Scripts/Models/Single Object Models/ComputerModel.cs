using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComputerModel : FurnitureModel
{
    //Public constructor using InGame data
    public ComputerModel(Vector3Int pos, float rot, FurnitureData data, ResourcesLoading.TileBasesName tileName) : base(pos, rot, tileName)
    {
        this.hp = data.maxHealth;
        this.cost = data.costDollar;
    }
}
