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
    public StringVariable nameOfTheTileBeneath;

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
        if (!SaveAndLoadController.IsRechargingFurnitures)
        {
            // get the floorTiles Name, then place the sprite accordinly
            ResourcesLoading.TileBasesName tileBeneath = (ResourcesLoading.TileBasesName)System.Enum.Parse(typeof(ResourcesLoading.TileBasesName), nameOfTheTileBeneath.theString);
            tileData.sprite = ResourcesLoading.FloorTileDic[tileBeneath].SpriteOfFloor;
        }
        //if we are reloading all furnitures based on the current models
        // recharging sprite
        else
        {
            FurnitureModel _model = FurnitureManager.Instance.GetModelFromAllDictionnaries(position);
            if (_model != null)
            {
                tileData.sprite = ResourcesLoading.FloorTileDic[_model.tileItWasPutOn].SpriteOfFloor;
            }
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

        // Create the office data class and add it to the total game data
        if (!SaveAndLoadController.IsRechargingFurnitures)
        {
            ResourcesLoading.TileBasesName tileBeneath = (ResourcesLoading.TileBasesName)System.Enum.Parse(typeof(ResourcesLoading.TileBasesName), nameOfTheTileBeneath.theString);
            FurnitureFactory.BuildFurniture(furnitureData, position, rotation.thefloat, go, tileBeneath);
        }
        //if we are reloading all furnitures based on the current models
        // do nothing as the FurnitureManagers contains already correct models
        else
        {
            //DO NOTHING
        }
        return true;
    }

}