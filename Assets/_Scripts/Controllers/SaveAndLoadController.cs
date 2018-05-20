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
    private string savePath = "/Resources/Save";

    // name of the saveFile
    private string saveFileName = "/mysave.scp";

    // Variables used by furnitures
    public FloatVariable rotationOfFurniture;

    // Used to fix RefreshTileBug
    public static bool IsLoading;

    public static SaveData loadedData;

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

    /**
     * Remove everything from the world tilema
     */
    private void ResetWorld()
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
     *  reset Everything
     */
    public void NewGame()
    {
        ResetWorld();
        // Clear All Managers
        FurnitureManager.Instance.ClearAll();
        WallsWithDoorManager.Instance.listOfAllWalls.Clear();
        FloorManager.Instance.listOfFloors.Clear();
        WallManager.Instance.listOfAllWalls.Clear();

        //TODO : We do not add back the jobs because not saveable 
        //JobManager.jobQueue.ClearAll();

        //TODO : add npcs !
        // Clear all NPCs
        foreach(var v in NPCManager.Instance.listOfNPCS.Values)
        {
            Destroy(v);
        }
        NPCManager.Instance.listOfNPCS.Clear();


    }

    /**
     *  Handle saving with an xml
     *  TODO : Save only  tilemaps and its go
     */
    public void Save()
    {
        Debug.Log("trying to save in the following file : " + completeSaveFilePath);
        FileStream saveFile = File.Open(completeSaveFilePath, FileMode.OpenOrCreate);

        SaveData data = new SaveData(worldTileMapRef,
            FloorManager.Instance,
            FurnitureManager.Instance,
            WallsWithDoorManager.Instance,
            WallManager.Instance,
            NPCManager.Instance);

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
            loadedData = (SaveData)bf.Deserialize(loadFile);
            loadFile.Close();

            Debug.Log("Trying to convert LoadData to map");

            // Reset the world
            ResetWorld();

            rotationOfFurniture.thefloat = 0;
            IsLoading = true;

            // Add the FloorModels before loading them
            LoadingFloorModels(loadedData);

            //Reload the world now with correct tiles
            // Put already correct sprite under furnitures
            worldTileMapRef = loadedData.ConvertDataToTileMap(worldTileMapRef);

            // Replace all existing models data
            //Furniture
            AssignGameObjectToAModel(loadedData);
            FurnitureManager.Instance.OnLoading();

            // FIX WALLS TOO

            //Walls with Doors
            AssignGameObjectsToWallWithDoorsModel(loadedData);

            // Add NPCs
            //AssignModelsAndGameObjectForNPCS(loadedData);

            // Build NavMesh
            NavMeshController.Instance.BuildNavMesh();

            // Add jobs
            //LoadAllJobsToQueue(loadedData);

            IsLoading = false;

        }
    }

    // Load all floor models and add them to the manager
    private void LoadingFloorModels(SaveData loadedData)
    {
        foreach(var v in loadedData.allFloors)
        {
            FloorManager.Instance.listOfFloors[v.GetTilePos()] = v;
        }
    }

    // Load all jobs to the queue (conversion from JobLite)
    private void LoadAllJobsToQueue(SaveData data)
    {
        foreach(var l in data.allJobsLite)
        {
            JobManager.jobQueue.Enqueue(new Job(l));
        }
    }

    // Add the NPC and its model in the world !
    private void AssignModelsAndGameObjectForNPCS(SaveData loadedData)
    {
        foreach(var v in loadedData.allNpcs)
        {
            NPCFactory.BuildNPCBasedOnModel(v);
        }
    }

    /**
     *  For furniture manager andWall manager
     */
    private void AssignGameObjectToAModel(SaveData loadData)
    {
        // Replace the model created by furnitureTile by the loaded one.
        foreach (var v in loadData.allOfficeModels)
        {
            FurnitureManager.OfficeObject _obj = FurnitureManager.Instance.listOfAllOffices[v.GetTilePos()];
            FurnitureManager.Instance.listOfAllOffices[v.GetTilePos()] = new FurnitureManager.OfficeObject(v, _obj.go);
        }

        foreach (var v in loadData.allComputerModels)
        {
            FurnitureManager.ComputerObject _obj = FurnitureManager.Instance.listOfAllComputers[v.GetTilePos()];
            FurnitureManager.Instance.listOfAllComputers[v.GetTilePos()] = new FurnitureManager.ComputerObject(v, _obj.go);
        }

        foreach (var v in loadData.allWall)
        {
            WallManager.WallObject _obj = WallManager.Instance.listOfAllWalls[v.GetTilePos()];
            WallManager.Instance.listOfAllWalls[v.GetTilePos()] = new WallManager.WallObject(v, _obj.go);
        }


    }

    /**
    *  For Wall with doors manager
     */
    private void AssignGameObjectsToWallWithDoorsModel(SaveData loadData)
    {
        // Replace the model created by WallWithDoorTile by the loaded one.
        WallsWithDoorManager.Instance.listOfAllWalls = UtilitiesMethod.ReplaceKeysOfDictByList<WallWithDoorsModel>(WallsWithDoorManager.Instance.listOfAllWalls, loadData.allWallsWithDoor);
    }

    /**
     * Get sprite under a furniture based on Vector3Int Position
     */
     public static ResourcesLoading.TileBasesName GetSpriteFromLoadedFurnitureList(Vector3Int pos, SaveData loadData)
    {

        FurnitureModel ret = null;
        //Office
        foreach (var model in loadData.allOfficeModels)
        {
            if (model.GetTilePos().Equals(pos))
            {
                ret = model;
            }
        }
        //Computers
        foreach (var model in loadData.allComputerModels)
        {
            if (model.GetTilePos().Equals(pos))
            {
                ret = model;
            }
        }

        if (ret != null)
        {
            return ret.tileItWasPutOn;
        }
        return ResourcesLoading.TileBasesName.Empty;
    }

    /**
 * Get sprite under a furniture based on Vector3Int Position
 */
    public static ResourcesLoading.TileBasesName GetSpriteFromLoadedWallsList(Vector3Int pos, SaveData loadData)
    {

        WallWithDoorsModel ret = null;
        //Wall with Doors
        foreach (var model in loadData.allWallsWithDoor)
        {
            if (model.GetTilePos().Equals(pos))
            {
                ret = model;
            }
        }

        if (ret != null)
        {
            return ret.tileItWasPutOn;
        }
        return ResourcesLoading.TileBasesName.Empty;
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

        // Wall with doors
        public List<WallWithDoorsModel> allWallsWithDoor;

        // Walls
        public List<WallModel> allWall;

        // All FloorTiles
        public List<FloorModel> allFloors;

        // List of All NPC models
        public List<NPCModel> allNpcs;

        // List of all jobs
        public List<JobLite> allJobsLite;

        
        /**
         *  Create the class which contains all saveable data
         */
        public SaveData(Tilemap map, FloorManager floorManager, FurnitureManager furnitureManager, WallsWithDoorManager wallsWithDoorManager, WallManager wallManager, NPCManager NpcManager)
        {
            // Save tilemap
            SaveMapTilesData(map);

            // Save Floor
            SaveFloorList(floorManager);

            //Save Furnitures
            SaveFurnituresDataList(map, furnitureManager);

            //Save Walls
            SaveWalls(map, wallsWithDoorManager, wallManager);

            //Save NPCs
            //SaveNPC(NpcManager);

            //Save Jobs
            //SaveJobs();

        }

        private void SaveFloorList(FloorManager floorManager)
        {
            allFloors = new List<FloorModel>();
            foreach(var v in floorManager.listOfFloors.Values)
            {
                allFloors.Add(v);
            }
        }

        private void SaveJobs()
        {
            // Only for Joblitebuildwalls for now
            allJobsLite = new List<JobLite>();

            while( JobManager.jobQueue.GetJobCount() >0 )
            {
                // big SWITCH CASE
                // because the TYPE of the job is X, we add a JobLiteBuildWall with the right argument ?
                // FIXME : where to stock the variability of the tile ?
                allJobsLite.Add(new JobLiteBuildWalls(JobManager.jobQueue.Dequeue(), ResourcesLoading.TileBasesName.Wall_Tile));
            }
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
        private void SaveFurnituresDataList(Tilemap map, FurnitureManager furnitureManager)
        {
            // TODO : optimize this ?

            allOfficeModels = new List<OfficeModel>();
            foreach (var v in furnitureManager.listOfAllOffices.Values)
            {
                allOfficeModels.Add(v.model);
            }

            allComputerModels = new List<ComputerModel>();
            foreach (var v in furnitureManager.listOfAllComputers.Values)
            {
                allComputerModels.Add(v.model);
            }
        }

        private void SaveWalls(Tilemap map, WallsWithDoorManager wallDManager, WallManager WallManager)
        {
            // To refacto like normal walls !
            allWallsWithDoor = UtilitiesMethod.GetListOfModelFromDictionnary<WallWithDoorsModel>(wallDManager.listOfAllWalls);

            //Walls
            allWall = new List<WallModel>();
            foreach (var v in WallManager.listOfAllWalls.Values)
            {
                allWall.Add(v.model);
            }
        }

        private void SaveNPC(NPCManager manager)
        {
            //Update v3 position of all NPCModels
            manager.UpdatePositionOfAllNPCModels();
            allNpcs = UtilitiesMethod.GetListOfModelFromDictionnary<NPCModel>(manager.listOfNPCS);
        }

    }

}
