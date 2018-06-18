using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

/**
 *  Contains all the different job actions 
 */
public class JobActions {


    // === WALL BUILDING
    // For Save
    public static UnityAction BuildWallJob(Tilemap map, Vector3Int npos)
    {
        return () =>
        {
            WallManager.Instance.listOfAllWalls[npos].model.isPending = false;
            WallManager.Instance.listOfAllWalls[npos].model.isPreview = false;
            //TODO not working
            WallManager.Instance.listOfAllWalls[npos].go.layer += 1;
            map.RefreshTile(npos);

        };
    }

    // For normal building
    public static Job BuildWallJob(Tilemap map, Vector3Int npos, WallTile tile)
    {
        return new Job(npos, () =>
        {
            WallManager.Instance.listOfAllWalls[npos].model.isPending = false;
            WallManager.Instance.listOfAllWalls[npos].model.isPreview = false;
            //TODO not working
            WallManager.Instance.listOfAllWalls[npos].go.layer += 1; 
            map.RefreshTile(npos);

        }, tile.wallData.buildingTime, "Wall");
    }

    // === FLOOR BUILDING
    // For Save
    public static UnityAction BuildFloorJob(Tilemap map, Vector3Int npos)
    {
        return () =>
        {
            FloorManager.Instance.listOfFloors[npos].isPending = false;
            FloorManager.Instance.listOfFloors[npos].isPreview = false;
            map.RefreshTile(npos);
        };
    }

    // For normal building
    public static Job BuildFloorJob(Tilemap map, Vector3Int npos, FloorTile tile)
    {
        return new Job(npos, () =>
        {
            // When job done, tile is not pending anymore and refresh it.
            FloorManager.Instance.listOfFloors[npos].isPending = false;
            FloorManager.Instance.listOfFloors[npos].isPreview = false;
            map.RefreshTile(npos);

        }, tile.buildingTime, "Floor");
    }

    // === FURNITURE BUILDING
    // For Save
    //TODO : change layer ?
    public static UnityAction BuildFurnitureJob(Tilemap map, Vector3Int npos)
    {
        return () =>
        {
            FurnitureManager.Instance.GetModelFromAllDictionnaries(npos).isPending = false;
            FurnitureManager.Instance.GetModelFromAllDictionnaries(npos).isPreview = false;
            map.RefreshTile(npos);
        };
    }

    // For normal building
    public static Job BuildFurnitureJob(Tilemap map, Vector3Int npos, FurnitureTile tile)
    {
        return new Job(npos, () =>
        {
            //Refresh based on the model
            FurnitureManager.Instance.GetModelFromAllDictionnaries(npos).isPending = false;
            FurnitureManager.Instance.GetModelFromAllDictionnaries(npos).isPreview = false;
            map.RefreshTile(npos);
        }, tile.furnitureData.buildingTime, "Furniture");
    }

}
