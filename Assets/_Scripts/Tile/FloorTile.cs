using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/**
 * Default Tile for the world
 * Contains floorSprite and furnitures
 */
[CreateAssetMenu(menuName = "Tile/Floor Tile")]
public class FloorTile : TileBase
{
    //The sprite of tile in the palette
    public Sprite SpriteOfFloor;
    public Sprite PendingSprite;

    // Time to build
    public float buildingTime;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        FloorModel _floor = FloorManager.Instance.listOfFloors[position];
        if (_floor.isPending)
        {
            tileData.sprite = PendingSprite;
        }
        else
        {
            tileData.sprite = SpriteOfFloor;
        }

        //Update the model
        _floor.nameOfFloor = tileData.sprite.name;

        base.GetTileData(position, tilemap, ref tileData);
    }

}
