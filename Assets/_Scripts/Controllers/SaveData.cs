using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    // List of all jobs from the JobManager
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
        SaveNPC(NpcManager);

        //Save Jobs
        SaveJobs();
    }

    private void SaveFloorList(FloorManager floorManager)
    {
        allFloors = new List<FloorModel>();
        foreach (var v in floorManager.listOfFloors.Values)
        {
            allFloors.Add(v);
        }
    }

    private void SaveJobs()
    {
        // Only for Joblitebuildwalls for now
        allJobsLite = new List<JobLite>();

        foreach (var v in JobManager.jobQueue.ConvertToJobList())
        {
            allJobsLite.Add(new JobLite(v));
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
        //Update v3 position and Job of all NPCModels
        manager.UpdatePositionAndJobLiteOfAllNPCModels();

        allNpcs = UtilitiesMethod.GetListOfModelFromDictionnary<NPCModel>(manager.listOfNPCS);
    }

}
