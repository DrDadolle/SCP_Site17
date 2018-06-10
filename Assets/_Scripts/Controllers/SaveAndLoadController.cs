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

        JobManager.jobQueue.ClearAll();

        //TODO : add npcs !
        // Clear all NPCs
        foreach(var v in NPCManager.Instance.listOfNPCS.Values)
        {
            Destroy(v);
        }
        NPCManager.Instance.listOfNPCS.Clear();


    }

    /**
     *  Save as a Binary
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

            // Reset the world
            ResetWorld();

            rotationOfFurniture.thefloat = 0;
            IsLoading = true;

            // == BEFORE LOADING 
            // Add the FloorModels before loading them
            LoadingFloorModels(loadedData);

            // Add the WallModels before loading them
            LoadingWallModels(loadedData);


            // == LOADING THE MAP
            //Reload the world now with correct tiles
            // Put already correct sprite under furnitures
            worldTileMapRef = loadedData.ConvertDataToTileMap(worldTileMapRef);

            // Replace all existing models data
            //Furniture
            AssignGameObjectToAModel(loadedData);
            FurnitureManager.Instance.OnLoading();

            //Walls with Doors
            AssignGameObjectsToWallWithDoorsModel(loadedData);

            // Add NPCs
            //AssignModelsAndGameObjectForNPCS(loadedData);

            // Build NavMesh
            NavMeshController.Instance.BuildNavMesh();

            // Add jobs
            LoadAllJobsToQueue(loadedData);

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

    // Load all Wall models and add them to the manager
    // The Game Object will be added in the WallTile Tilebase Class
    private void LoadingWallModels(SaveData loadedData)
    {
        foreach (var v in loadedData.allWall)
        {
            WallManager.WallObject _w = new WallManager.WallObject(v, null);
            WallManager.Instance.listOfAllWalls[v.GetTilePos()] = _w;
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
    }

    /**
    *  For Wall with doors manager
     */
    private void AssignGameObjectsToWallWithDoorsModel(SaveData loadData)
    {
        // TODO : Replace the model created by WallWithDoorTile by the loaded one.
        WallsWithDoorManager.Instance.listOfAllWalls = UtilitiesMethod.ReplaceKeysOfDictByList<WallWithDoorsModel>(WallsWithDoorManager.Instance.listOfAllWalls, loadData.allWallsWithDoor);

        // Normal Walls
        foreach (var v in loadData.allWall)
        {
            WallManager.WallObject _obj = WallManager.Instance.listOfAllWalls[v.GetTilePos()];
            WallManager.Instance.listOfAllWalls[v.GetTilePos()] = new WallManager.WallObject(v, _obj.go);
        }
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

}
