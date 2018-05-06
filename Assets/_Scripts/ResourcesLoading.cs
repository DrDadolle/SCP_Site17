using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Class containing the loaded resources (e.g. Materials)
 */
[System.Serializable]
public class ResourcesLoading : MonoBehaviour {

    // Materials
    protected static string PATH_TO_GHOST_MATERIALS = "Materials_Ghost";
    public enum MaterialNames
    {
        Walls,
        Ghost_Blue,
        Ghost_Red,
        MousePointerCube_Blue,
        MousePointerCube_Red
    }
    public static Dictionary<MaterialNames, Material> MaterialDictionnary = new Dictionary<MaterialNames, Material>();

    //Tiles
    protected static string PATH_TO_FLOOR_TILES = "Floor_Tiles";
    public enum FloorTilesName
    {
        DarkGrey_Tile,
        Orange_Tile,
        Empty_Tile
    }
    public static Dictionary<FloorTilesName, Tile> FloorTilesDictionnary = new Dictionary<FloorTilesName, Tile>();

    //Walls Prefab
    protected static string PATH_TO_WALL_PREFAB = "Test_Wall";
    public enum WallsPrefabName
    {
        X_Wall,
        Corner_Wall,
        I_Wall,
        T_Wall,
        Door_Wall,
        Window_Wall
    }
    public static Dictionary<WallsPrefabName, GameObject> WallPrefabDictionnary = new Dictionary<WallsPrefabName, GameObject>();

    // TileBases
    protected static string PATH_TO_TILEBASE = "Tile";
    [System.Serializable]
    public enum TileBasesName
    {
        //For special cases
        Empty,
        DarkGrey_Floor_Tile,
        Orange_Floor_Tile,
        Computer,
        Furniture_Tile_Test,
        Wall_Tile,
        Test_Wall_Tile_With_Floor
    }
    // One dictionnary per TileBase child class
    public static Dictionary<TileBasesName, FurnitureTile> FurnitureTileDic      = new Dictionary<TileBasesName, FurnitureTile>();
    public static Dictionary<TileBasesName, FloorTile> FloorTileDic              = new Dictionary<TileBasesName, FloorTile>();
    public static Dictionary<TileBasesName, WallTile> WallTileDic                = new Dictionary<TileBasesName, WallTile>();
    public static Dictionary<TileBasesName, WallWithDoorTile> WallDoorTileDic    = new Dictionary<TileBasesName, WallWithDoorTile>();




    // Awake
    void Awake () {
        //Loading Materials for Ghostly effects 
        LoadingMaterials();

        //Loading Tiles sprites
        LoadingTiles();

        //Loading Walls Prefabs
        LoadingWallPrefabs();

        // Loading All type of TileBase
        LoadingTileBases();
    }


    //Loading tiles
    private void LoadingTiles()
    {
        Object[] tmp = Resources.LoadAll(PATH_TO_FLOOR_TILES, typeof(Tile));
        string[] names = System.Enum.GetNames(typeof(FloorTilesName));
        FloorTilesName[] values = (FloorTilesName[])System.Enum.GetValues(typeof(FloorTilesName));

        foreach (var t in tmp)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (t.name.Equals(names[i]))
                    FloorTilesDictionnary.Add(values[i], (Tile)t);
            }

        }

        //Check that the loading was successful
        if (FloorTilesDictionnary.Count != tmp.Length)
            Debug.LogError("Loading of FloorTiles failed");

    }


    //Loading tiles
    private void LoadingTileBases()
    {
        Object[] tmp = Resources.LoadAll(PATH_TO_TILEBASE, typeof(TileBase));
        string[] names = System.Enum.GetNames(typeof(TileBasesName));
        TileBasesName[] values = (TileBasesName[])System.Enum.GetValues(typeof(TileBasesName));

        foreach (var t in tmp)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (t.name.Equals(names[i]))
                    if(t is FloorTile)
                        FloorTileDic.Add(values[i], (FloorTile)t);
                    else if (t is FurnitureTile)
                        FurnitureTileDic.Add(values[i], (FurnitureTile)t);
                    else if (t is WallTile)
                        WallTileDic.Add(values[i], (WallTile)t);
                    else if (t is WallWithDoorTile)
                        WallDoorTileDic.Add(values[i], (WallWithDoorTile)t);
            }

        }

        //Check that the loading was successful
        if (FloorTileDic.Count + FurnitureTileDic.Count + WallTileDic.Count + WallDoorTileDic.Count != tmp.Length)
            Debug.LogError("Loading of TileBase failed");

    }

    //Loading tiles
    private void LoadingWallPrefabs()
    {
        Object[] tmp = Resources.LoadAll(PATH_TO_WALL_PREFAB, typeof(GameObject));
        string[] names = System.Enum.GetNames(typeof(WallsPrefabName));
        WallsPrefabName[] values = (WallsPrefabName[])System.Enum.GetValues(typeof(WallsPrefabName));

        foreach (var t in tmp)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (t.name.Equals(names[i]))
                    WallPrefabDictionnary.Add(values[i], (GameObject)t);
            }

        }

        //Check that the loading was successful
        if (WallPrefabDictionnary.Count != tmp.Length)
            Debug.LogError("Loading of Wall Prefabs failed");

    }

    // Loading Ghostly Materials
    private void LoadingMaterials()
    {
        Object[] tmp_materials = Resources.LoadAll(PATH_TO_GHOST_MATERIALS, typeof(Material));

        string[] names = System.Enum.GetNames(typeof(MaterialNames));
        MaterialNames[] values = (MaterialNames[])System.Enum.GetValues(typeof(MaterialNames));

        foreach (var t in tmp_materials)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if(t.name.Equals(names[i]))
                    MaterialDictionnary.Add(values[i], (Material)t);
            }
            
        }

        //Check that the loading was successful
        if (MaterialDictionnary.Count != tmp_materials.Length)
            Debug.LogError("Loading of Ghostly Materials failed");
    }

}
