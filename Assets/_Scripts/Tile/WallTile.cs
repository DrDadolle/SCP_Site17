using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Tile type handling walls
 */
[CreateAssetMenu(menuName = "Tile/Wall Tile")]
public class WallTile : TileBase
{
    //The data
    public WallData wallData;

    //Rotation value (in degree) of the prefab to align them correctly
    private float rotPrefab;
    public Shader HologramShader;

    /**
     * Is called whenever a tile at "position" is placed
     */
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //For some reasons, it is needed to real time update the preview walls
        // if (SaveAndLoadController.IsLoading)
        //{
        base.RefreshTile(position, tilemap);
        //}

        CommonMethodWall.RefreshNeighbourWallsMethod(position, tilemap);
    }

    /**
     * Usually called by RefreshTile methods
     * Update the sprite and the GO
     */
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        string composition = CommonMethodWall.ReturnCompositionOfNearWalls(position, tilemap);

        //Add the wall prefab based on the composition
        rotPrefab = SetWallBasedOnComposition(composition, ref tileData);

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

        WallModel wallModel = WallManager.Instance.listOfAllWalls[position].model;
        //Add the gameobject to dictionnary of WallManager
        WallManager.WallObject _obj = new WallManager.WallObject(wallModel, go);
        WallManager.Instance.listOfAllWalls[position] = _obj;

        if (wallModel.isPending)
        {

            UtilitiesMethod.ChangeShaderOfRecChildGameObject(go, HologramShader);
        }
        else
        {
            UtilitiesMethod.ChangeShaderOfRecChildGameObject(go, ResourcesLoading.ShaderDic[ResourcesLoading.ShaderNames.Basic]);
        }

        return true;
    }

    /**
     *  handle Exhaustively the composition cases
     *  return the rotation
    */
    private float SetWallBasedOnComposition(string composition, ref TileData tileData)
    {
        float rot = 0f;
        GameObject prefab;
        

        // Only one neighbour situation
        if (composition.Equals("EEEW") || composition.Equals("EEWE") || composition.Equals("EWEE") || composition.Equals("WEEE"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.I_Wall];
            // Z axis alignement need rotation
            if (composition.Equals("EEWE") || composition.Equals("EEEW"))
            {
                rot = 90f;
            }
        }
        //Corner neighbour situation
        else if (composition.Equals("EWWE") || composition.Equals("EWEW") || composition.Equals("WEEW") || composition.Equals("WEWE"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.Corner_Wall];

            if (composition.Equals("WEEW"))
            {
                rot = 90f;
            }
            else if (composition.Equals("EWEW"))
            {
                rot = 180f;
            }
            else if (composition.Equals("EWWE"))
            {
                rot = 270f;
            }
        }
        // Opposite neighbour
        else if (composition.Equals("WWEE") || composition.Equals("EEWW"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.I_Wall];
            // Z axis alignement need rotation
            if (composition.Equals("EEWW"))
            {
                rot = 90f;
            }

        }
        //T crossing
        else if (composition.Equals("WWWE") || composition.Equals("WWEW") || composition.Equals("WEWW") || composition.Equals("EWWW"))
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.T_Wall];

            if (composition.Equals("WEWW"))
            {
                rot = 90f;
            }
            else if (composition.Equals("WWEW"))
            {
                rot = 180f;
            }
            else if (composition.Equals("EWWW"))
            {
                rot = 270f;
            }
        }
        //No neighbour or 4 neighbour = default prefab
        else
        {
            prefab = ResourcesLoading.WallPrefabDictionnary[ResourcesLoading.WallsPrefabName.X_Wall];
        }

        //Adding the wall prefab on the tile
        tileData.gameObject = prefab;

        return rot;
    }

}