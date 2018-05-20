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


    //TODO : write all actions
    // For Save
    public static UnityAction BuildWallJob(Tilemap map, Vector3Int npos)
    {
        return () =>
        {
            WallManager.Instance.listOfAllWalls[npos].model.isPending = false;
            WallManager.Instance.listOfAllWalls[npos].model.isPreview = false;
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
            map.RefreshTile(npos);

        }, tile.wallData.buildingTime);
    }

}
