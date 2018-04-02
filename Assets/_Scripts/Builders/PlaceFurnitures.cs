using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceFurnitures : MonoBehaviour, IPlaceObjectMethod {

    //Reference to the tilemap
    public Tilemap map;

    // Reference to the FurnitureTile
    //It is empty
    public FurnitureTile tile;

    // for access purpose
    public static PlaceFurnitures Instance;

    // Store in memory the sprite of the previous FloorTile
    public Sprite OldFloorSprite {
        get; private set;
    }

    // Store in memory the wanted Rotation
    //TODO : Reset to zero when leaving construction mode
    public float RotationFurniture
    {
        get; private set;
    }



    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IPlaceObjectMethod ==============================
    // These methods are called by the MouseController

    public void OnKeyboardPress()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            RotationFurniture += 90;
            RotationFurniture = RotationFurniture % 360;
        }
    }

    public void OnLeftButtonPress(MouseController pointer)
    {
        UpdateTile(pointer);
    }
    // ============================== Implement IPlaceObjectMethod ==============================

    private void UpdateTile(MouseController pointer)
    {
        Vector3Int tilePos = map.WorldToCell(pointer.getWorldPoint());

        //If we are not over a floorTile, bail out
        if (!(map.GetTile(tilePos) is FloorTile))
            return;

        //Get Old sprite
        OldFloorSprite = ((FloorTile)map.GetTile(tilePos)).SpriteOfFloor;

        //Place the furniture
        map.SetTile(tilePos, tile);
    }
}
