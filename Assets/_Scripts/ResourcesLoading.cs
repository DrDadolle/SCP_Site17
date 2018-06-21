using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Class containing the loaded resources (e.g. Materials)
 */
public class ResourcesLoading : MonoBehaviour {

    //FIXME : to be fixed !
    public GameObject UglyWorkAroundToGetStandardShader;

    // Materials
    protected static string PATH_TO_SHADER = "Shaders";
    public enum ShaderNames
    {
        BlueHologram,
        Basic
    }
    public static Dictionary<ShaderNames, Shader> ShaderDic = new Dictionary<ShaderNames, Shader>();

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

    // NPC Prefab
    protected static string PATH_TO_NPC_PREFAB = "NPC_Prefab";
    public enum NPCPrefabNames
    {
        Scientist,
        Construction_Engineer
    }
    public static Dictionary<NPCPrefabNames, GameObject> NPCPrefabDic = new Dictionary<NPCPrefabNames, GameObject>();


    // Awake
    void Awake () {
        //Loading Materials for Ghostly effects 
        LoadingShaders();

        //Loading Tiles sprites
        LoadingTiles();

        //Loading Walls Prefabs
        LoadingWallPrefabs();

        // Loading All type of TileBase
        LoadingTileBases();

        //Loading NPC data
        LoadingNPCPrefab();
    }

    //LoadingNPCData
    private void LoadingNPCPrefab()
    {
        GameObject[] tmp = Resources.LoadAll<GameObject>(PATH_TO_NPC_PREFAB);
        string[] names = System.Enum.GetNames(typeof(NPCPrefabNames));
        NPCPrefabNames[] values = (NPCPrefabNames[])System.Enum.GetValues(typeof(NPCPrefabNames));

        foreach (var t in tmp)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (t.name.Equals(names[i]))
                    NPCPrefabDic.Add(values[i], t);
            }

        }

        //Check that the loading was successful
        if (NPCPrefabDic.Count != tmp.Length)
            Debug.LogError("Loading of NPC Prefabs failed. Found : " + NPCPrefabDic.Count + ", expected : " + tmp.Length);
    }


    //Loading tiles
    private void LoadingTiles()
    {
        UnityEngine.Object[] tmp = Resources.LoadAll(PATH_TO_FLOOR_TILES, typeof(Tile));
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
        UnityEngine.Object[] tmp = Resources.LoadAll(PATH_TO_TILEBASE, typeof(TileBase));
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
        UnityEngine.Object[] tmp = Resources.LoadAll(PATH_TO_WALL_PREFAB, typeof(GameObject));
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

    // Loading Shaders
    private void LoadingShaders()
    {
        UnityEngine.Object[] tmp_shader = Resources.LoadAll(PATH_TO_SHADER, typeof(Shader));
        string[] names = Enum.GetNames(typeof(ShaderNames));
        ShaderNames[] values = (ShaderNames[])Enum.GetValues(typeof(ShaderNames));

        foreach (var t in tmp_shader)
        {
            for (int i = 0; i < names.Length; i++)
            {
                // taking only the end of the name because name are like "graphs/XXX" or "hidden/YYY"
                if(t.name.Split('/')[1].Equals(names[i]))
                    ShaderDic.Add(values[i], (Shader)t);
            }
            
        }

        //FIXME : WORKAROUND
        ShaderDic[ShaderNames.Basic] = UglyWorkAroundToGetStandardShader.GetComponentsInChildren<Renderer>()[0].sharedMaterial.shader;

        //Check that the loading was successful
        // 2 hidden shader
        if (ShaderDic.Count != tmp_shader.Length - 1)
            Debug.LogError("Loading of Shaders failed");
    }

}
