using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 *  Will manage state of all Floors
 */
public class FloorManager : MonoBehaviour {

    // For access purpose
    public static FloorManager Instance;

    //Map of the world
    public Tilemap world;

    // Dictionnary of all the gameobjects furnitures
    public Dictionary<Vector3Int, FloorModel> listOfFloors;

    // OnAwake
    void Awake()
    {
        listOfFloors = new Dictionary<Vector3Int, FloorModel>();
        Instance = this;
    }

}
