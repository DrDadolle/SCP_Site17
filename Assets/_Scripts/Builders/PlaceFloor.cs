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

    // List of all pending tiles
    private List<Vector3Int> listOfPendingTiles = new List<Vector3Int>();

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

    private List<Job> listOfPotentialJobs = new List<Job>();

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

        // Add all jobs
        foreach (var j in listOfPotentialJobs)
        {
            JobManager.jobQueue.Enqueue(j);
        }
        // Change the preview tile to the pending tiles
        foreach (var v in listOfPendingTiles)
        {
            FloorManager.Instance.listOfFloors[v].isPending = true;
            FloorManager.Instance.listOfFloors[v].isPreview = true;
            map.RefreshTile(v);
        }

        listOfPotentialJobs.Clear();
        listOfPendingTiles.Clear();
    }

    public void OnRightButtonPressDuringDragAndDrop(TileBase tile)
    {
        CleanUpPreviewTiles();
        listOfPotentialJobs.Clear();
        listOfPendingTiles.Clear();
    }


    public void OnKeyboardPress(TileBase tile)
    {
        // Do nothing
    }

    public void OnUpdateWhenTileIsChanged(TileBase tile)
    {
        // TODO : Show preview ?
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
        ClearAll();



        // Display a preview of the drag area
        for (int x = start_x; x <= end_x; x++)
        {
            for (int y = start_y; y <= end_y; y++)
            {
                Vector3Int npos= new Vector3Int(x, y, 0);

                //Add the old tile to the list
                OldTileData t_tmp = new OldTileData(npos, map.GetTile(npos));
                listOfOldTiles.Add(t_tmp);

                //Set the pending tile
                // Add model to FloorManager
                bool wasEmpty = false; ;
                if(!FloorManager.Instance.listOfFloors.ContainsKey(npos))
                {
                    wasEmpty = true;
                    FloorManager.Instance.listOfFloors.Add(npos, new FloorModel(npos, tile.SpriteOfFloor.name, true, false));
                }

                FloorModel _model = FloorManager.Instance.listOfFloors[npos];

                // If the floor behind is Pending, or the same type : do nothing
                bool conditionOfPlacingFloors = !_model.isPending && (_model.nameOfFloor != tile.SpriteOfFloor.name  || wasEmpty);

                if (conditionOfPlacingFloors)
                {
                    map.SetTile(npos, tile);
                    listOfPendingTiles.Add(npos);

                    /**
                    * JOBS !
                    * We should create all the jobs only on mousebutton release !
                    * We should create a tmp list of jobs !
                    */
                    Job j_tmp = new Job(npos, (theJob) =>
                    {
                        // When job done, tile is not pending anymore and refresh it.
                        FloorManager.Instance.listOfFloors[npos].isPending = false;
                        FloorManager.Instance.listOfFloors[npos].isPreview = false;
                        map.RefreshTile(npos);

                    }, tile.buildingTime);

                    listOfPotentialJobs.Add(j_tmp);
                }
            }
        }
    }

    /** Remove the previewTiles and add back the old one
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

    private void ClearAll()
    {
        // Clean up old drag previews
        CleanUpPreviewTiles();
        listOfPotentialJobs.Clear();
        //Remove all preview Walls Models before clearing tiles
        foreach (var v in listOfPendingTiles)
        {
            FloorManager.Instance.listOfFloors.Remove(v);
        }
        listOfPendingTiles.Clear();
    }


}
