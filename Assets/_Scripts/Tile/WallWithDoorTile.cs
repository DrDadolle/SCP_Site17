using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 *  WallTile with a hole for fit a door
 */
[CreateAssetMenu(menuName = "Tile/Wall with Door Tile")]
public class WallWithDoorTile : TileBase
{
    //Reference to the prefab
    public GameObject Wall_With_A_Hole_Prefab;

    //The sprite
    private string OldFloorTileName;
    public DictionnaryVariable dictionnaryOfOldTiles;

    private float rotPrefab;

    /**
     *  Overiding the refresh Tile
     *  TODO : optimise the refresh to avoid chain refresh :(
     */
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        CommonMethodWall.RefreshNeighbourWallsMethod(position, tilemap);

        if (SaveAndLoadController.IsLoading)
        {
            base.RefreshTile(position, tilemap);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        string composition = CommonMethodWall.ReturnCompositionOfNearWalls(position, tilemap);
        rotPrefab = 0f;

        if (composition.Equals("WWEE") || composition.Equals("EEWW"))
        {
            // Z axis alignement need rotation
            if (composition.Equals("EEWW"))
            {
                rotPrefab = 90f;
            }

        }
        //Handle go
        tileData.gameObject = Wall_With_A_Hole_Prefab;

        //Normal behaviour
        if (!SaveAndLoadController.IsLoading)
        {
            Vector3 v3 = new Vector3(position.x, position.y, position.z);
            // if contains, get the name, else it is "Empty"
            if (dictionnaryOfOldTiles.theDict.ContainsKey(v3))
            {
                OldFloorTileName = dictionnaryOfOldTiles.theDict[v3];
            }
            // get the floorTiles Name, then place the sprite accordinly
            ResourcesLoading.TileBasesName tileBeneath = (ResourcesLoading.TileBasesName)System.Enum.Parse(typeof(ResourcesLoading.TileBasesName), OldFloorTileName);
            tileData.sprite = ResourcesLoading.FloorTileDic[tileBeneath].SpriteOfFloor;
        }
        //if we are reloading all WallWithDoors based on the current models
        // recharging sprite
        else
        {
            ResourcesLoading.TileBasesName tileBeneath = SaveAndLoadController.GetSpriteFromLoadedWallsList(position, SaveAndLoadController.loadedData);
            tileData.sprite = ResourcesLoading.FloorTileDic[tileBeneath].SpriteOfFloor;
            OldFloorTileName = tileBeneath.ToString();
        }

        base.GetTileData(position, tilemap, ref tileData);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {       
        //Center the position of the game object
        //By default the position starts at Left bottom point of the cell
        // TODO need to add anchor points  (vertical and horisontal (left,centre,right)(top,centre,bottom))
        go.transform.position += Vector3.right * 0.5f + Vector3.forward * 0.5f;

        //Handle rotation based on the composition
        go.transform.Rotate(Vector3.up * rotPrefab);

        // Create the WallWithDoor data class and add it to the total game data

        //Debug.Log("RUN TWICE BUG LOL");
        ResourcesLoading.TileBasesName tileBeneath = (ResourcesLoading.TileBasesName)System.Enum.Parse(typeof(ResourcesLoading.TileBasesName), OldFloorTileName);
        WallWithDoorsModel _model = new WallWithDoorsModel(position, tileBeneath);

        if (!WallsWithDoorManager.Instance.listOfAllWalls.ContainsKey(_model))
            WallsWithDoorManager.Instance.listOfAllWalls.Add(new WallWithDoorsModel(position, tileBeneath), go);

        return true;
    }



}
