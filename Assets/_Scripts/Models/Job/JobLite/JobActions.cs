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
            map.RefreshTile(npos);
            // Change the layer from GhostWalls to Walls
            WallManager.Instance.listOfAllWalls[npos].go.layer = 10;

        };
    }

    // For normal building
    public static Job BuildWallJob(Tilemap map, Vector3Int npos, WallTile tile)
    {
        return new Job(npos, () =>
        {
            WallManager.Instance.listOfAllWalls[npos].model.isPending = false;
            WallManager.Instance.listOfAllWalls[npos].model.isPreview = false;
            map.RefreshTile(npos);
            // Change the layer from GhostWalls to Walls
            WallManager.Instance.listOfAllWalls[npos].go.layer = 10;

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
    public static UnityAction BuildFurnitureJob(Tilemap map, Vector3Int npos)
    {
        return () =>
        {
            FurnitureModel _model = FurnitureManager.Instance.GetModelFromAllDictionnaries(npos);
            _model.isPending = false;
            _model.isPreview = false;
            map.RefreshTile(npos);
            // Change the layer from GhostFurnitures to Furniture
            FurnitureManager.Instance.GetGameObjectFromAllDictionnaries(npos).layer = 12;
        };
    }

    // For normal building
    public static Job BuildFurnitureJob(Tilemap map, Vector3Int npos, FurnitureTile tile)
    {
        return new Job(npos, () =>
        {
            //Refresh based on the model
            FurnitureModel _model = FurnitureManager.Instance.GetModelFromAllDictionnaries(npos);
            _model.isPending = false;
            _model.isPreview = false;
            map.RefreshTile(npos);
            // Change the layer from GhostFurnitures to Furniture
            FurnitureManager.Instance.GetGameObjectFromAllDictionnaries(npos).layer = 12;
        }, tile.furnitureData.buildingTime, "Furniture");
    }

}
