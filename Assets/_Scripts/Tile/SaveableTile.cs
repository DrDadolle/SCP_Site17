using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/**
 * Every tile in the game extends this
 */
 [Serializable]
public class SaveableTile{

    // Workaround
    public int x;
    public int y;
    public int z;

    //Workaround because Tilebase is not serializable
    public ResourcesLoading.TileBasesName tiletype;

    public SaveableTile(Vector3Int vec3, ResourcesLoading.TileBasesName type)
    {
        x = vec3.x;
        y = vec3.y;
        z = vec3.z;
        tiletype = type;
    }

}
