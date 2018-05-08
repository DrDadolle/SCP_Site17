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

    // Dictionnary of all the gameobjects furnitures
    public Dictionary<WallModel, GameObject> listOfAllWalls;

    // OnAwake
    void Awake()
    {
        listOfAllWalls = new Dictionary<WallModel, GameObject>();
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

    /**
     *  Get the model based on position from all the dictionnary
    */
    public WallModel GetWallModelFromDict(Vector3Int pos)
    {
        WallModel ret = null;
        //Walls
        foreach (var model in listOfAllWalls.Keys)
        {
            if (model.GetTilePos().Equals(pos))
            {
                return model;
            }
        }
        return ret;
    }

    /**
     * Check if there is a wallModel at the position
     */
     public bool CheckIfWallExistsAtPosition(Vector3Int pos)
    {
        return (GetWallModelFromDict(pos) != null);
    }

    /**
     * Remove wallmodel from dict
     */
     public void RemoveWallModelFromDict(Vector3Int pos)
    {
        listOfAllWalls.Remove(GetWallModelFromDict(pos));
    }

}
