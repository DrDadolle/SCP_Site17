using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableModels/Furniture Data")]
public class FurnitureData : ScriptableObject {

    //The name
    public string nameOfFurniture;

    // How much it cost to place
    public float costDollar;

    //Max health of the furniture
    public float maxHealth;

    // The prefab in itself
    public GameObject furniturePrefab;

    // Where you can put this on
    public List<FloorTile> tilesItCanBePlacedOn;
}
