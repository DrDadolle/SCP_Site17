using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableModels/Wall Data")]
public class WallData : ScriptableObject
{

    //The name
    public string nameOfWall;

    // How much it cost to place
    public float costDollar;

    //Max health of the furniture
    public float maxHealth;

    // The prefab in itself
    // TODO : not required for now
    // Should be a list of prefab ??? to handle all corners ?
    //public GameObject furniturePrefab;
}
