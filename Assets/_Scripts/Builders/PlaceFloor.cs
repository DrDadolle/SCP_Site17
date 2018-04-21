using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceFloor : MonoBehaviour, IBuildingMethod
{

    // Tilemap containing the world
    public Tilemap map;

    // Instance for access purpose
    public static PlaceFloor Instance;

    // Store the ref to the mouse pointer
    public MousePointer pointer;

    // Starting Cell position of the drag and drop
    private Vector3Int startpos;

    // Current Cell position of the drag and drop
    private Vector3Int currentpos;

    // Storing old positions
    public class OldTileData
    {
        public Vector3Int npos;
        public TileBase tileBase;

        public OldTileData(Vector3Int npos, TileBase tileBase)
        {
            this.npos = npos;
            this.tileBase = tileBase;
        }
    }
    private List<OldTileData> listOfOldTiles = new List<OldTileData>();



    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    // Set the starting position of the drag & drop
    public void OnLeftButtonPress(TileBase tile)
    {
        //Save starting position
        startpos = map.WorldToCell(pointer.GetWorldPoint());
    }

    public void DuringDragAndDrop(TileBase tile)
    {

        UpdateTile((FloorTile)tile);
    }


    public void OnLeftButtonReleaseDuringDragAndDrop(TileBase tile)
    {
        // Clean up the memory
        listOfOldTiles.Clear();
    }

    public void OnRightButtonPressDuringDragAndDrop(TileBase tile)
    {
        CleanUpPreviewTiles();
    }


    public void OnKeyboardPress()
    {
        //DO NOTHING
    }

    // ========================== End Implement IPlacingFloorTile ==============================


    private void UpdateTile(FloorTile tile)
    {
        // Avoid calling the updatetile method every frame if nothing changed
        if (currentpos == map.WorldToCell(pointer.GetWorldPoint()))
            return;

        // get current position
        currentpos = map.WorldToCell(pointer.GetWorldPoint());

        // Get starting and end position
        int start_x = startpos.x;
        int end_x = currentpos.x;
        int start_y = startpos.y;
        int end_y = currentpos.y;

        // We may be dragging in the "wrong" direction, so flip things if needed.
        if (end_x < start_x)
        {
            int tmp = end_x;
            end_x = start_x;
            start_x = tmp;
        }
        if (end_y < start_y)
        {
            int tmp = end_y;
            end_y = start_y;
            start_y = tmp;
        }

        /**
         * Now start_x start_y is smaller to the end point
         */

        // Clean up old drag previews
        CleanUpPreviewTiles();


        // Display a preview of the drag area
        for (int x = start_x; x <= end_x; x++)
        {
            for (int y = start_y; y <= end_y; y++)
            {
                Vector3Int npos= new Vector3Int(x, y, 0);

                //Add the old tile to the list
                OldTileData t_tmp = new OldTileData(npos, map.GetTile(npos));
                listOfOldTiles.Add(t_tmp);

                // Display the building hint on top of this tile position
                map.SetTile(npos, tile);
            }
        }
    }

    /** Remove the previewTiles and add back the old one
     * FIXME special case with furnitures
     */
    private void CleanUpPreviewTiles()
    {
        while (listOfOldTiles.Count > 0)
        {
            OldTileData t = listOfOldTiles[0];
            listOfOldTiles.RemoveAt(0);
            map.SetTile(t.npos, t.tileBase);
        }
    }

}
