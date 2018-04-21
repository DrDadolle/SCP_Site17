using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveAndLoadController : MonoBehaviour {

    // The world map
    public Tilemap worldTileMapRef;

    // where we save the tilemaps
    private string directorySaving;
    private string completeSaveFilePath;

    // path of the savefiles
    private string savePath = "/Resources/SavedMaps";

    // name of the saveFile
    private string saveFileName = "/mysave.dat";

    // Variables used by furnitures
    public FloatVariable rotationOfFurniture;

    // Used to fix RefreshTileBug
    public static bool IsLoading;
    public static bool IsRechargingFurnitures;

    //On awake
    private void Awake()
    {
        directorySaving = Application.persistentDataPath + savePath;
        completeSaveFilePath = directorySaving + saveFileName;

        if (Directory.Exists(directorySaving) == false)
        {
            Directory.CreateDirectory(directorySaving);
            Debug.Log("Creating Directory for saving maps");

            AssetDatabase.Refresh();
        }
    }

    // Remove everything from the world tilemap
    public void ResetWorld()
    {
        //TODO : When more complexe objects and data will be created, do not forget to reset then
        // Does not clear characters
        // Removes tiles sprite
        worldTileMapRef.ClearAllTiles();

        // Removes child gameobjects of the tiles
        for (var i = 0; i < worldTileMapRef.transform.childCount; i++)
        {
            Destroy(worldTileMapRef.transform.GetChild(i).gameObject);
        }
    } 

    /**
     *  Handle saving with an xml
     *  TODO : Save only  tilemaps and its go
     */
    public void Save()
    {
        Debug.Log("trying to save in the following file : " + completeSaveFilePath);
        FileStream saveFile = File.Open(completeSaveFilePath, FileMode.OpenOrCreate);

        SaveData data = new SaveData(worldTileMapRef, FurnitureManager.Instance);

        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(saveFile, data);

        saveFile.Close();
    }

    /**
     *   Handle load
     */
    public void Load()
    {
        if(File.Exists(completeSaveFilePath))
        {
            Debug.Log("trying to load the following file : " + completeSaveFilePath);
            FileStream loadFile = File.Open(completeSaveFilePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            SaveData loadData = (SaveData)bf.Deserialize(loadFile);
            loadFile.Close();

            Debug.Log("Trying to convert LoadData to map");

            // Reset the world
            ResetWorld();
            // Clear the furniture Manager
            FurnitureManager.Instance.ClearAll();

            rotationOfFurniture.thefloat = 0;
            IsLoading = true;

            //Recharge the world to recreate all Tiles and update their managers
            worldTileMapRef = loadData.ConvertDataToTileMap(worldTileMapRef);
            // Reset the world
            ResetWorld();

            // Replace all existing models !
            AssignGameObjectToAModel(loadData);

            //Reload the world now with correct tiles
            IsRechargingFurnitures = true;
            worldTileMapRef = loadData.ConvertDataToTileMap(worldTileMapRef);
            IsRechargingFurnitures = false;

            //Rotate
            FurnitureManager.Instance.OnLoading();
            IsLoading = false;

        }
    }

     private void AssignGameObjectToAModel(SaveData loadData)
    {
        // Replace the model created by furnitureTile by the loaded one.
        FurnitureManager.Instance.listOfAllOffices =  FurnitureManager.Instance.ReplaceKeysOfDictByList<OfficeModel>(FurnitureManager.Instance.listOfAllOffices, loadData.allOfficeModels);
        FurnitureManager.Instance.listOfAllComputers = FurnitureManager.Instance.ReplaceKeysOfDictByList<ComputerModel>(FurnitureManager.Instance.listOfAllComputers, loadData.allComputerModels);
    }

    /**
     * This class will save :
     *  - All tiles
     *  - All go linked to tiles
     *  
     *  Furnitures :
     *      - Offices
     * 
     */
    [Serializable]
    public class SaveData
    {
        // All the tiles
        public List<SaveableTile> allTiles;

        // All models of the furnitures
        public List<OfficeModel> allOfficeModels;
        public List<ComputerModel> allComputerModels;
        
        /**
         *  Create the class which contains all saveable data
         */
        public SaveData(Tilemap map, FurnitureManager furnitureManager)
        {
            // Save tilemap
            SaveMapTilesData(map);

            //Save Furnitures
            SavaFurnituresDataList(map, furnitureManager);
        }

        private void SaveMapTilesData(Tilemap map)
        {
            allTiles = new List<SaveableTile>();

            //Get Array of all existing TileBase
            BoundsInt bounds = map.cellBounds;
            TileBase[] _allTiles = map.GetTilesBlock(bounds);
            // for all tiles
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    // If the tile exists and isn't null
                    TileBase tile = _allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        Vector3Int npos = new Vector3Int(x + bounds.position.x, y + bounds.position.y, 0);
                        //Convert the name of the tile to the TileBasesNames from the resources loading class
                        //Array containing all possibles names of tiles & enums
                        string[] names = Enum.GetNames(typeof(ResourcesLoading.TileBasesName));
                        ResourcesLoading.TileBasesName[] values = (ResourcesLoading.TileBasesName[])System.Enum.GetValues(typeof(ResourcesLoading.TileBasesName));

                        //Find the enum type according to the tile name
                        for (var i = 0; i < names.Length; i++)
                        {
                            if (tile.name.Equals(names[i]))
                            {
                                allTiles.Add(new SaveableTile(npos, values[i]));
                            }
                        }

                    }
                }
            }
        }

        /**
         *  Return the saved tilemap
         */
        public Tilemap ConvertDataToTileMap(Tilemap worldtilemap)
        {

            // Load the tiles
            foreach (var t in allTiles)
            {
                Vector3Int vpos = new Vector3Int(t.x, t.y, t.z);
                // Check if the tile is located in every dictionnary
                if (ResourcesLoading.FloorTileDic.ContainsKey(t.tiletype))
                {
                    FloorTile ft = (FloorTile)ResourcesLoading.FloorTileDic[t.tiletype];
                    worldtilemap.SetTile(vpos, ft);
                }
                else if (ResourcesLoading.FurnitureTileDic.ContainsKey(t.tiletype))
                {
                    FurnitureTile ft = (FurnitureTile)ResourcesLoading.FurnitureTileDic[t.tiletype];
                    worldtilemap.SetTile(vpos, ft);
                }
                if (ResourcesLoading.WallTileDic.ContainsKey(t.tiletype))
                {
                    WallTile wt = (WallTile)ResourcesLoading.WallTileDic[t.tiletype];
                    worldtilemap.SetTile(vpos, wt);
                }
                if (ResourcesLoading.WallDoorTileDic.ContainsKey(t.tiletype))
                {
                    WallWithDoorTile wwdt = (WallWithDoorTile)ResourcesLoading.WallDoorTileDic[t.tiletype];
                    worldtilemap.SetTile(vpos, wwdt);
                }
            }
            return worldtilemap;
        }


        /**
         *  Save the data of furnitures
         */
        private void SavaFurnituresDataList(Tilemap map, FurnitureManager furnitureManager)
        {
            allOfficeModels = furnitureManager.GetListOfModelFromDictionnary<OfficeModel>(furnitureManager.listOfAllOffices);
            allComputerModels = furnitureManager.GetListOfModelFromDictionnary<ComputerModel>(furnitureManager.listOfAllComputers);
        }

    }

}
