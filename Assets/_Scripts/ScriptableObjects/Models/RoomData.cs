using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableModels/Room Data")]
public class RoomData : ScriptableObject {

    //The name
    public string NameOfRoom;

    // Is it a military type of Room ?
    // Scientific ?
    // Containement type ?
    // Office type ?
    public RoomTypes.Type typeOfRoom;

    public WallTile defaultWallTile;

    // 
    public FloorTile theFloorTile;

    // What kind of furnitures It can have !
    public List<FurnitureTile> listOfFurnitures;



}
