using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Tile type handling walls
 */
[CreateAssetMenu]
public class WallTile : TileBase
{   
    
    //Rotation value (in degree) of the prefab to align them correctly
    private float rotPrefab;

    /**
     * Indicates if the wall has a hole in it (to place doors)
     */
    public bool HasAHole
    {
        get; private set;
    }

    /**
     * Is called whenever a tile at "position" is placed
     */
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {     
        //Check horizontal neighbours
        for (int x = -1; x <= 1; x++)
        {
            Vector3Int nPos = new Vector3Int(position.x + x, position.y, position.z);
            RefreshNeighbourWallTiles(tilemap, nPos);

        }

        //Check vertical neighbours
        //Note : it is Y because tilemap are XY based
        for (int y = -1; y <= 1; y++)
        {
            Vector3Int nPos = new Vector3Int(position.x, position.y + y, position.z);
            RefreshNeighbourWallTiles(tilemap, nPos);
        }

        base.RefreshTile(position, tilemap);
    }

    /**
     * Usually called by RefreshTile methods
     * Update the sprite and the GO
     */
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
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
                if(HasWall(tilemap, nPos))
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
                if (HasWall(tilemap, nPos))
                {
                    composition += "W";
                }
                else
                {
                    composition += "E";
                }
            }
        }

        //Add the wall prefab based on the composition
        SetWallBasedOnComposition(composition, ref tileData);


        //Delete the sprite below the wall
        tileData.sprite = null;
    }

    /**
     * Called on the first frame of the scene
     */
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //Center the position of the game object
        //By default the position starts at Left bottom point of the cell
        // TODO need to add anchor points  (vertical and horisontal (left,centre,right)(top,centre,bottom))
        go.transform.position += Vector3.right * 0.5f + Vector3.forward * 0.5f ;

        //Handle rotation based on the composition
        go.transform.Rotate(Vector3.up * rotPrefab);

        return true;
    }

    /**
     * Refresh all neighbour wall tiles
     */
    private void RefreshNeighbourWallTiles(ITilemap tilemap, Vector3Int pos)
    {
        if(HasWall(tilemap,pos))
        {
            tilemap.RefreshTile(pos);
        }
    }

    /**
     * Return true if the tile is a walltile
     */
    private bool HasWall(ITilemap tilemap, Vector3Int pos)
    {
        return tilemap.GetTile(pos) == this;
    }

    /**
     *  handle Exhaustively the composition cases
     */
    private void SetWallBasedOnComposition(string composition, ref TileData tileData)
    {
        rotPrefab = 0f;
        GameObject prefab;

        // Only one neighbour situation
        if(composition.Equals("EEEW") || composition.Equals("EEWE") || composition.Equals("EWEE") || composition.Equals("WEEE"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.I_Wall];
            // Z axis alignement need rotation
            if (composition.Equals("EEWE") || composition.Equals("EEEW"))
            {
                rotPrefab = 90f;
            }
        }
        //Corner neighbour situation
        else if(composition.Equals("EWWE") || composition.Equals("EWEW") || composition.Equals("WEEW") || composition.Equals("WEWE"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.Corner_Wall];

            if (composition.Equals("WEEW"))
            {
                rotPrefab = 90f;
            }
            else if (composition.Equals("EWEW"))
            {
                rotPrefab = 180f;
            }
            else if (composition.Equals("EWWE"))
            {
                rotPrefab = 270f;
            }
        }
        // Opposite neighbour
        else if(composition.Equals("WWEE") || composition.Equals("EEWW"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.I_Wall];
            // Z axis alignement need rotation
            if (composition.Equals("EEWW"))
            {
                rotPrefab = 90f;
            }

        }
        //T crossing
        else if (composition.Equals("WWWE") || composition.Equals("WWEW") || composition.Equals("WEWW") || composition.Equals("EWWW"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.T_Wall];

            if (composition.Equals("WEWW"))
            {
                rotPrefab = 90f;
            }
            else if (composition.Equals("WWEW"))
            {
                rotPrefab = 180f;
            }
            else if (composition.Equals("EWWW"))
            {
                rotPrefab = 270f;
            }
        }
        //No neighbour or 4 neighbour = default prefab
        else
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.X_Wall];
        }

        //Adding the wall prefab on the tile
        tileData.gameObject = prefab;
    }



}