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

    public SpriteVariable oldSpriteOfFloor;

    private float rotPrefab;

    /**
     *  Overiding the refresh Tile
     */
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        CommonMethodWall.RefreshNeighbourWallsMethod(position, tilemap);

        base.RefreshTile(position, tilemap);
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

        //Handle sprite
        tileData.sprite = oldSpriteOfFloor.sprite;

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

        return true;
    }



}
