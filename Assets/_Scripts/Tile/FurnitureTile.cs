using UnityEngine;
using UnityEngine.Tilemaps;


/**
 * Handling furnitures
 * Contains floorSprite and furnitures
 */
[CreateAssetMenu]
public class FurnitureTile : TileBase
{
    public GameObject furniturePrefab;

    /**
     * Usually called by RefreshTile methods
     * Update the sprite and the GO
     */
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Adding the wall prefab on the tile
        tileData.gameObject = furniturePrefab;
        tileData.sprite = PlaceFurnitures.Instance.OldFloorSprite;

        base.GetTileData(position, tilemap, ref tileData);
    }

    /**
     * Called on the first frame of the scene
     */
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //Center the position of the game object
        //By default the position starts at Left bottom point of the cell
        // TODO need to add anchor points  (vertical and horisontal (left,centre,right)(top,centre,bottom))
        go.transform.position += Vector3.right * 0.5f + Vector3.forward * 0.5f;
        go.transform.Rotate(Vector3.up * PlaceFurnitures.Instance.RotationFurniture);

        return true;
    }

}