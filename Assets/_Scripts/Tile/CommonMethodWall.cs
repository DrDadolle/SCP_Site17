using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CommonMethodWall {

    //Rotation value (in degree) of the prefab to align them correctly
    private float rotPrefab;

    /**
     *  Call refresh on all neighbour walls automatically
     */
    public static void RefreshNeighbourWallsMethod(Vector3Int position, ITilemap tilemap)
    {
        //Check horizontal neighbours
        for (int x = -1; x <= 1; x++)
        {
            // Do not refresh itself !!!!
            if (x != 0)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y, position.z);
                RefreshNeighbourWallTiles(tilemap, nPos);
            }

        }

        //Check vertical neighbours
        //Note : it is Y because tilemap are XY based
        for (int y = -1; y <= 1; y++)
        {
            // Do not refresh itself !!!!
            if (y != 0)
            {
                Vector3Int nPos = new Vector3Int(position.x, position.y + y, position.z);
                RefreshNeighbourWallTiles(tilemap, nPos);
            }
        }

    }

    /**
    * Refresh all neighbour wall tiles
    */
    private static void RefreshNeighbourWallTiles(ITilemap tilemap, Vector3Int pos)
    {
        if (HasWall(tilemap, pos))
        {
            //TODO : add the same for WallWithDoorTile

            Debug.Log("Pending : " + tilemap.GetTile<WallTile>(pos).isPending);
            //The following line is buggy :
            tilemap.RefreshTile(pos);
            Debug.Log("Pending : " + tilemap.GetTile<WallTile>(pos).isPending);
        }
    }

    /**
    * Return true if the tile is a walltile
    */
    public static bool HasWall(ITilemap tilemap, Vector3Int pos)
    {
        return (tilemap.GetTile(pos) is WallTile || tilemap.GetTile(pos) is WallWithDoorTile);
    }

    /**
* Return true if the tile is a walltile
*/
    public static bool HasWall(Tilemap tilemap, Vector3Int pos)
    {
        return (tilemap.GetTile(pos) is WallTile || tilemap.GetTile(pos) is WallWithDoorTile);
    }

    /**
     * return the composition of the nearby tiles only for wall or wall with door
     *  Composition consist of W for Wall tiles and E for non wall tiles
     *      | 4 |
     *  -------------
     *    1 |   | 2
     *  -------------  
     *      | 3 |
     */
    public static string ReturnCompositionOfNearWalls(Vector3Int position, ITilemap tilemap)
    {
        string composition = "";

        /** Check if the neighbour has walls
         * Check horizontal neighbours
         */
        for (int x = -1; x <= 1; x++)
        {
            if (x != 0)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y, position.z);
                if (CommonMethodWall.HasWall(tilemap, nPos))
                {
                    composition += "W";
                }
                else
                {
                    composition += "E";
                }
            }

        }

        /** Check if the neighbour has walls
         * Check vertical neighbours
         * Note : it is Y because tilemap are XY based
         */
        for (int y = -1; y <= 1; y++)
        {
            if (y != 0)
            {
                Vector3Int nPos = new Vector3Int(position.x, position.y + y, position.z);
                if (CommonMethodWall.HasWall(tilemap, nPos))
                {
                    composition += "W";
                }
                else
                {
                    composition += "E";
                }
            }
        }

        return composition;
    }

    /**
     * return the composition of the nearby tiles only for wall or wall with door
     *  Composition consist of W for Wall tiles and E for non wall tiles
     *      | 4 |
     *  -------------
     *    1 |   | 2
     *  -------------  
     *      | 3 |
     */
    public static string ReturnCompositionOfNearWalls(Vector3Int position, Tilemap tilemap)
    {
        string composition = "";

        /** Check if the neighbour has walls
         * Check horizontal neighbours
         */
        for (int x = -1; x <= 1; x++)
        {
            if (x != 0)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y, position.z);
                if (CommonMethodWall.HasWall(tilemap, nPos))
                {
                    composition += "W";
                }
                else
                {
                    composition += "E";
                }
            }

        }

        /** Check if the neighbour has walls
         * Check vertical neighbours
         * Note : it is Y because tilemap are XY based
         */
        for (int y = -1; y <= 1; y++)
        {
            if (y != 0)
            {
                Vector3Int nPos = new Vector3Int(position.x, position.y + y, position.z);
                if (CommonMethodWall.HasWall(tilemap, nPos))
                {
                    composition += "W";
                }
                else
                {
                    composition += "E";
                }
            }
        }

        return composition;
    }


}
