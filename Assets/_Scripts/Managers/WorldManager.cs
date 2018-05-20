using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour {

    public Tilemap world;

    public static WorldManager Instance;

    // On Awake
    private void Awake()
    {
        Instance = this;
    }
}
