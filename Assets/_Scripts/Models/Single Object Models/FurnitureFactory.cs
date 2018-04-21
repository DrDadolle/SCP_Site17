using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 *  Factory of furnitures
 */
static public class FurnitureFactory {

    /**
     *  Create the furniture model and add it to the furniture manager !
     */
    public static void BuildFurniture(FurnitureData data, Vector3Int pos, float rotation, GameObject go, ResourcesLoading.TileBasesName tileName)
    {
        switch(data.nameOfFurniture)
        {
            case "Office":
                FurnitureManager.Instance.listOfAllOffices.Add(new OfficeModel(pos, rotation, data, tileName), go);
                break;
            case "Computer":
                FurnitureManager.Instance.listOfAllComputers.Add(new ComputerModel(pos, rotation, data, tileName), go);
                break;
        }
    }

}