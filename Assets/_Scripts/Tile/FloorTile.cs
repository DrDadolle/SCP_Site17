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


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = SpriteOfFloor;
        base.GetTileData(position, tilemap, ref tileData);
    }

}
