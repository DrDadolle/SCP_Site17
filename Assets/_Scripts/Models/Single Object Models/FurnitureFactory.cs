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
    public static void BuildFurniture(FurnitureData data, Vector3Int pos, float rotation, GameObject go, ResourcesLoading.TileBasesName tileName, bool preview, bool pending)
    {
        switch(data.nameOfFurniture)
        {
            case "Office":
                FurnitureManager.OfficeObject _off = new FurnitureManager.OfficeObject(new OfficeModel(pos, rotation, data, tileName, preview, pending), go);
                FurnitureManager.Instance.listOfAllOffices.Add(pos, _off);
                break;
            case "Computer":
                FurnitureManager.ComputerObject _com = new FurnitureManager.ComputerObject(new ComputerModel(pos, rotation, data, tileName, preview, pending), go);
                FurnitureManager.Instance.listOfAllComputers.Add(pos, _com);
                break;
        }
    }
}