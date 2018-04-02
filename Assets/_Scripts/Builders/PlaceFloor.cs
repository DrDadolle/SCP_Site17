using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceFloor : MonoBehaviour, IPlacingFloorTile {

    // Tilemap containing the world
    public Tilemap map;

    // Instance for access purpose
    public static PlaceFloor Instance;

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

    // ============================== Implement IPlacingFloorTile ==============================
    // These methods are called by the MouseController

    // Set the starting position of the drag & drop
    public void OnLeftButtonPress(MouseController pointer, FloorTile tile)
    {
        //Save starting position
        startpos = map.WorldToCell(pointer.getWorldPoint());
    }

    public void DuringDragAndDrop(MouseController pointer, FloorTile tile)
    {

        UpdateTile(pointer, tile);
    }


    public void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer, FloorTile tile)
    {
        // Clean up the memory
        listOfOldTiles.Clear();
    }

    public void OnRightButtonPressDuringDragAndDrop(MouseController pointer, FloorTile tile)
    {
        ResetOldTiles(pointer, tile);
    }

    // ========================== End Implement IPlacingFloorTile ==============================


    private void UpdateTile(MouseController pointer, FloorTile tile)
    {
        // Avoid calling the updatetile method every frame if nothing changed
        if (currentpos == map.WorldToCell(pointer.getWorldPoint()))
            return;

        // get current position
        currentpos = map.WorldToCell(pointer.getWorldPoint());

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

    // Remove the preview tiles
    private void ResetOldTiles(MouseController pointer, FloorTile tile)
    {
        CleanUpPreviewTiles();
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
