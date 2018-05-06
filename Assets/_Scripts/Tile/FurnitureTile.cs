using UnityEngine;
using UnityEngine.Tilemaps;


/**
 * Handling furnitures
 * Contains floorSprite and furnitures
 */
[CreateAssetMenu(menuName = "Tile/Furniture Tile")]
public class FurnitureTile : TileBase
{
    //The prefab
    public FurnitureData furnitureData;

    //The sprite
    private string OldFloorTileName;
    public DictionnaryVariable dictionnaryOfOldTiles;

    // Ref to the rotation float
    public FloatVariable rotation;


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        if(SaveAndLoadController.IsLoading)
        {
            base.RefreshTile(position, tilemap);
        }
       //DO NOTHING
    }

    /**
     * Usually called by RefreshTile methods
     * Update the sprite and the GO
     */
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Adding the wall prefab on the tile
        tileData.gameObject = furnitureData.furniturePrefab;

        //Normal behaviour
        if (!SaveAndLoadController.IsLoading)
        {
            Vector3 v3 = new Vector3(position.x, position.y, position.z);
            // if contains, get the name, else it is "Empty"
            if(dictionnaryOfOldTiles.theDict.ContainsKey(v3))
            {
                OldFloorTileName = dictionnaryOfOldTiles.theDict[v3];
            }
            else
            {
                OldFloorTileName = "Empty";
            }

            // get the floorTiles Name, then place the sprite accordinly
            ResourcesLoading.TileBasesName tileBeneath = (ResourcesLoading.TileBasesName)System.Enum.Parse(typeof(ResourcesLoading.TileBasesName), OldFloorTileName);
            tileData.sprite = ResourcesLoading.FloorTileDic[tileBeneath].SpriteOfFloor;
        }
        //if we are reloading all furnitures based on the current models
        // recharging sprite
        else
        {
            ResourcesLoading.TileBasesName tileBeneath = SaveAndLoadController.GetSpriteFromLoadedFurnitureList(position, SaveAndLoadController.loadedData);
            tileData.sprite = ResourcesLoading.FloorTileDic[tileBeneath].SpriteOfFloor;
            OldFloorTileName = tileBeneath.ToString();
        }
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
        go.transform.Rotate(Vector3.up * rotation.thefloat);
        go.name = furnitureData.nameOfFurniture + " : " + position.x + "_" + position.y;

        // Create the furniture data class and add it to the total game data
        ResourcesLoading.TileBasesName tileBeneath = (ResourcesLoading.TileBasesName)System.Enum.Parse(typeof(ResourcesLoading.TileBasesName), OldFloorTileName);
        FurnitureFactory.BuildFurniture(furnitureData, position, rotation.thefloat, go, tileBeneath);
        return true;
    }

}