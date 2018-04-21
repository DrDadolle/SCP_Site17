using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OfficeModel : FurnitureModel
{
    public string researcherUsingIt;
    public SCPClassification.Classification ResearchClassification;

    //Public constructor using InGame data
    public OfficeModel(Vector3Int pos, float rot, FurnitureData data, ResourcesLoading.TileBasesName tileName) : base(pos, rot, tileName)
    {
        this.hp = data.maxHealth;
        this.cost = data.costDollar;
        this.researcherUsingIt = "";
        this.ResearchClassification = SCPClassification.Classification.Safe;
    }
}
