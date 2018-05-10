using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 *  Will manage state of all Walls
 */
public class WallManager : MonoBehaviour {


    // For access purpose
    public static WallManager Instance;

    //Map of the world
    public Tilemap world;

    // Struct of the OfficeObject
    public struct WallObject
    {
        public WallModel model;
        public GameObject go;

        public WallObject(WallModel m, GameObject g)
        {
            model = m;
            go = g;
        }
    }

    // Dictionnary of all the gameobjects furnitures
    public Dictionary<Vector3Int, WallObject> listOfAllWalls;

    // OnAwake
    void Awake()
    {
        listOfAllWalls = new Dictionary<Vector3Int, WallObject>();
        Instance = this;
    }


    /**
     *  Clear all Walls data
     */
    public void ClearAll()
    {
        listOfAllWalls.Clear();
    }

    /**
    *  Called when GameObjects got loaded
    *  TODO : make it generic
    */
    public void OnLoading()
    {

    }

}
