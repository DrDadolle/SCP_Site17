using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class ConstructionController : MonoBehaviour
{
    // ScriptableObject of the building Mode
    public BooleanVariables areWeBuilding;

    //Reference to the GameObjectVariable MousePointer
    public MousePointer mousePointer;
    private Vector3 lastMousePosition;

    // Ref to the rotation float
    public FloatVariable rotationOfFurniture;

    //Tile of the floor we want to place on the tilemap
    private TileBase tb;

    //Storing the building function called
    // Need to update this based on the IBuildingMethod Interface methods
    private delegate void BuildingObjects(TileBase tile);
    BuildingObjects OnLeftButtonPressMethod;
    BuildingObjects DuringDragAndDropMethod;
    BuildingObjects OnLeftButtonReleaseDuringDragAndDropMethod;
    BuildingObjects OnRightButtonPressDuringDragAndDropMethod;
    BuildingObjects OnUpdateWhenTileIsChangedMethod;
    BuildingObjects OnKeyboardPressMethod;

    //Indicate if currently drag&dropping
    private bool dragAndDropping;

    //Keep in memory the last mode called
    // Created as  : "NAME_OF_METHOD:NAME_OF_TILE"
    private string lastBuildingModeUsed = "";
    private string currentBuildingModeUsed = "";
    private bool isFirstCallSinceChangeMode;


    // used for access purposes
    public static ConstructionController Instance;


    //On Awake
    void Awake () {
        Instance = this;
        lastMousePosition = Vector3.zero;
        areWeBuilding.theboolean = false;
    }
	
	// Update is called once per frame
	void Update () {

        //If we are over a UI element, then don't call a building method unless we are already dragging
        if (EventSystem.current.IsPointerOverGameObject() && !dragAndDropping)
        {
            return;
        }

        if (!lastBuildingModeUsed.Equals(currentBuildingModeUsed))
        {
            isFirstCallSinceChangeMode = true;
            lastBuildingModeUsed = currentBuildingModeUsed;
        }

        if(areWeBuilding.theboolean)
            CallingBuildingAction(tb);

        //Change the mode of construction
        if (areWeBuilding.theboolean && Input.GetMouseButtonUp(1))
        {
            Set_Mode_Build_To_Not_Building();
        }
    }


    //Method calling the different case of building method
    public void CallingBuildingAction(TileBase tile)
    {
        if (isFirstCallSinceChangeMode)
        {
            // Changing the material of the pointer
            mousePointer.go.GetComponent<Renderer>().material.shader = (ResourcesLoading.ShaderDic[ResourcesLoading.ShaderNames.BlueHologram]);

            //Reset rotation of furniture
            rotationOfFurniture.thefloat = 0.0f;

            isFirstCallSinceChangeMode = false;
        }

        // Update once each time the tile of the cursor is changed
        if (mousePointer.go.transform.position != lastMousePosition)
        {
            lastMousePosition = mousePointer.go.transform.position;
            OnUpdateWhenTileIsChangedMethod(tile);
        }

        //When clicking for the first time,
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftButtonPressMethod(tile);
            dragAndDropping = true;
        }

        //FIXME because not very clean
        if(Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            OnKeyboardPressMethod(tile);
        }

        //During a drap and drop
        if (dragAndDropping)
        {
            //If right click is pressed to cancel
            if (Input.GetMouseButtonDown(1))
            {
                //Drag&Drop Cancel
                dragAndDropping = false;
                OnRightButtonPressDuringDragAndDropMethod(tile);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Finishing the drag and drop
                dragAndDropping = false;
                OnLeftButtonReleaseDuringDragAndDropMethod(tile);
            }
            else
            {
                //Continue Drag&Dropping
                DuringDragAndDropMethod(tile);
            }
        }
    }


    //Sets the building mode to BuildingWall;
    public void Set_Mode_Build_Wall(WallTile tile)
    {
        areWeBuilding.theboolean = true;
        SetBuildingMethods(BuildWall.Instance);
        currentBuildingModeUsed = typeof(BuildWall).ToString() + ":" + tile.name;
        // Select the tile
        this.tb = tile;
    }

    //Sets the building mode to BuildingWall;
    public void Set_Mode_Build_Wall_Will_Door()
    {
        areWeBuilding.theboolean = true;
        SetBuildingMethods(BuildWallWithDoor.Instance);
        currentBuildingModeUsed = typeof(BuildWallWithDoor).ToString() + ":";
    }

    //Sets the building mode to BuildingObjects
    public void Set_Mode_Place_Furnitures(FurnitureTile tile)
    {
        areWeBuilding.theboolean = true;
        SetBuildingMethods(PlaceFurnitures.Instance);
        currentBuildingModeUsed = typeof(PlaceFurnitures).ToString() + ":" + tile.name;
        // Select the tile
        this.tb = tile;
    }

    //Sets the building mode to PlacingFloorTiles
    public void Set_Mode_Placing_Floor(FloorTile tile)
    {
        areWeBuilding.theboolean = true;
        SetBuildingMethods(PlaceFloor.Instance);
        currentBuildingModeUsed = typeof(PlaceFloor).ToString() + ":" + tile.name;
        // Select the tile
        this.tb = tile;
    }

    //Sets the building mode to Bulldoze
    public void Set_Mode_Build_To_Not_Building()
    {
        areWeBuilding.theboolean = false;
        currentBuildingModeUsed = "";
    }

    //Sets the building mode to NotBuilding
    public void Set_Mode_Build_To_Bulldoze()
    {
        areWeBuilding.theboolean = true;
        SetBuildingMethods(Bulldoze.Instance);
        currentBuildingModeUsed = typeof(Bulldoze).ToString() + ":";
    }

    // Utility Method Setting all the IBuildingMethods to the BuildingGameObjects var
    private void SetBuildingMethods(IBuildingMethod buildingMethod)
    {
        OnLeftButtonPressMethod = buildingMethod.OnLeftButtonPress;
        DuringDragAndDropMethod = buildingMethod.DuringDragAndDrop;
        OnLeftButtonReleaseDuringDragAndDropMethod = buildingMethod.OnLeftButtonReleaseDuringDragAndDrop;
        OnRightButtonPressDuringDragAndDropMethod = buildingMethod.OnRightButtonPressDuringDragAndDrop;
        OnKeyboardPressMethod = buildingMethod.OnKeyboardPress;
        OnUpdateWhenTileIsChangedMethod = buildingMethod.OnUpdateWhenTileIsChanged;
    }
}
