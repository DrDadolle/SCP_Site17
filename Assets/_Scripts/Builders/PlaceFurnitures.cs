using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceFurnitures : MonoBehaviour, IBuildingMethod
{

    //Reference to the tilemap
    public Tilemap map;

    // Store the ref to the mouse pointer
    public MousePointer pointer;

    // for access purpose
    public static PlaceFurnitures Instance;

    // Ref to the rotation float
    public FloatVariable rotation;

    //The sprite dict
    public DictionnaryVariable dictionnaryOfOldTiles;

    //List of pending job
    private List<Job> listOfPotentialJobs = new List<Job>();

    // List of all pending tiles
    // Will be used for MultiTiles Objects
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
    private OldTileData OldTile = new OldTileData(Vector3Int.zero, null);

    // On Awake
    void Awake()
    {
        Instance = this;
    }

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    public void OnKeyboardPress(TileBase tile)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rotation.thefloat += 90;
            rotation.thefloat = rotation.thefloat % 360;
            PreviewFurniture(tile as FurnitureTile);
        }
    }

    public void OnLeftButtonPress(TileBase tile)
    {
        UpdateTile(tile as FurnitureTile);
    }

    public void OnLeftButtonReleaseDuringDragAndDrop(TileBase tile)
    {
        //Do nothing
    }

    public void DuringDragAndDrop(TileBase tile)
    {
        //Do nothing
    }

    public void OnRightButtonPressDuringDragAndDrop(TileBase tile)
    {
        //Do nothing
    }

    public void OnUpdateWhenTileIsChanged(TileBase tile)
    {
        PreviewFurniture(tile as FurnitureTile);
    }

    // ============================== Implement IPlaceObjectMethod ==============================

    private void PreviewFurniture(FurnitureTile tile)
    {
        //Remove previous model and add back the other one
        Vector3Int previousPosition = OldTile.npos;

        //Remove old previews models
        FurnitureModel _fm = FurnitureManager.Instance.GetModelFromAllDictionnaries(previousPosition);
        if (_fm != null && !_fm.isPending && _fm.isPreview)
        {
            FurnitureManager.Instance.RemoveModelFromDictionnaries(previousPosition, true, false);

            map.SetTile(previousPosition, OldTile.tileBase);
            dictionnaryOfOldTiles.RemoveFromDict(previousPosition);
        }

        Vector3Int tilePos = map.WorldToCell(pointer.GetWorldPoint());

        //FIXME : for now, we do not override furnitures
        TileBase _tb = map.GetTile(tilePos);
        if (_tb == null || !(_tb is FloorTile) || (FloorManager.Instance.listOfFloors[tilePos].isPending))
            return;

        //FIXME : large furnitures
        OldTile = new OldTileData(tilePos, _tb);

        //Check if Tile Placeable
        CheckIsThisFurniturePlaceable(tile, tilePos);

        //Create the model and add it to the Furnituremanager
        // GO and Tile are null. Furniture Tile will handle them later
        // TODO : move it instead of creating/Deletion ? #Opti
        FurnitureFactory.BuildFurniture(tile.furnitureData, tilePos, 0f, null, ResourcesLoading.TileBasesName.Empty, true, false);

        //Set the furniture tile on the map
        map.SetTile(tilePos, tile);
    }


    private void UpdateTile(FurnitureTile tile)
    {
        Vector3Int tilePos = map.WorldToCell(pointer.GetWorldPoint());

        // IF there is already a pending furniture, bail out
        if (FurnitureManager.Instance.GetModelFromAllDictionnaries(tilePos).isPending)
            return;

        //Put preview :
        FurnitureManager.Instance.GetModelFromAllDictionnaries(tilePos).isPending = true;
        FurnitureManager.Instance.GetModelFromAllDictionnaries(tilePos).isPreview = false;
        map.RefreshTile(tilePos);


        /**
        * JOBS !
        * We should create all the jobs only on mousebutton release !
        * We should create a tmp list of jobs !
        */
        Job j_tmp = new Job(tilePos, (theJob) =>
        {
            //Refresh based on the model
            FurnitureManager.Instance.GetModelFromAllDictionnaries(tilePos).isPending = false;
            FurnitureManager.Instance.GetModelFromAllDictionnaries(tilePos).isPreview = false;
            map.RefreshTile(tilePos);
        }, tile.furnitureData.buildingTime);

        JobManager.jobQueue.Enqueue(j_tmp);
    }


    // Check if we can put this furniture there
    //If we are not over a valid floorTile, bail out
    private void CheckIsThisFurniturePlaceable(FurnitureTile tile, Vector3Int tilePos)
    {
        //If not a FloorTile, bail out
        if (!(map.GetTile(tilePos) is FloorTile))
            return;

        if (!tile.furnitureData.tilesItCanBePlacedOn.Contains(map.GetTile(tilePos) as FloorTile))
        {
            // Bail out
            Debug.Log("Can't place " + tile.furnitureData.furniturePrefab.name + " on " + map.GetTile(tilePos).name + " tile !");
            return;
        }

        //Save old sprite
        Vector3 v3 = new Vector3(tilePos.x, tilePos.y, tilePos.z);
        // If it contains already this tile
        if (!dictionnaryOfOldTiles.AddToDict(v3, ((FloorTile)map.GetTile(tilePos)).name))
        {
            Debug.LogError("Trying to add twice the old tile data. Return.");
            return;
        }
    }
}
