using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 *  Will manage state of all Walls with doors
 */
public class WallsWithDoorManager : MonoBehaviour {

    // For access purpose
    public static WallsWithDoorManager Instance;

    //Map of the world
    public Tilemap world;

    // Dictionnary of all the gameobjects furnitures
    public Dictionary<WallWithDoorsModel, GameObject> listOfAllWalls;

    // OnAwake
    void Awake()
    {
        listOfAllWalls = new Dictionary<WallWithDoorsModel, GameObject>();
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

    internal WallWithDoorsModel GetModelFromAllDictionnaries(Vector3Int position)
    {
        WallWithDoorsModel ret = null;
        //Office
        foreach (var model in listOfAllWalls.Keys)
        {
            if (model.GetTilePos().Equals(position))
            {
                return model;
            }
        }
        return ret;
    }
}
