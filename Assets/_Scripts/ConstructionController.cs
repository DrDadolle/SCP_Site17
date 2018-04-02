using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class ConstructionController : MonoBehaviour {

	public static BuildingMode CurrentBuildingMode {
        get; private set;
    }
    public static GlobalBuildingMode CurrentGlobalBuildingMode {
        get; private set;
    }

	public enum BuildingMode {
		NotBuilding,
		BuildingWall,
		BuildingObjects,
        PlacingFloor,
        Bulldoze
	};

    public enum GlobalBuildingMode
    {
        NotBuilding,
        BuildingWall,
        PlacingFurnitures,
        PlacingFloor,
        Bulldoze
    };

    //FIXME : Move it else where
    //Tile of the floor we want to place on the tilemap
    private FloorTile ftile;

    //Storing the building function called
    // Need to update this based on the IBuildingMethod Interface methods
    private delegate void BuildingGameObjects(MouseController pointer);
    BuildingGameObjects onLeftButtonPressBMethod;
    BuildingGameObjects duringDragAndDropBMethod;

    //Storing the place object function called
    // Need to update this based on the IPlaceObjectmethod Interface methods
    private delegate void PlacingGameObjects(MouseController pointer);
    PlacingGameObjects onLeftButtonPressPMethod;

    private delegate void PlacingGamesObjects_withoutPointer();
    PlacingGamesObjects_withoutPointer OnKeyboardPressPMethod;

    //Storing the placing floor function called
    // Need to update this based on the IPlacingFloorTile Interface methods
    private delegate void PlacingFloorTiles(MouseController pointer, FloorTile tile);
    PlacingFloorTiles onLeftButtonPressFMethod;
    PlacingFloorTiles duringDragAndDropFMethod;
    PlacingFloorTiles onLeftButtonReleaseDuringDragAndDropFMethod;
    PlacingFloorTiles onRightButtonPressDuringDragAndDropFMethod;

    //Storing the Bulldoze function called
    // Need to update this based on the IBulldozeMethod Interface methods
    private delegate void BulldozeDel(MouseController pointer);
    BulldozeDel onLeftButtonPressBulldozeMethod;


    //Indicate if currently drag&dropping
    private bool dragAndDropping;

    // used for access purposes
    public static ConstructionController Instance;


    //On Awake
    void Awake () {
        Instance = this;
		CurrentBuildingMode = BuildingMode.NotBuilding;
        CurrentGlobalBuildingMode = GlobalBuildingMode.NotBuilding;
	}
	
	// Update is called once per frame
	void Update () {

        //Change the mode of construction
        if (CurrentGlobalBuildingMode != GlobalBuildingMode.NotBuilding && Input.GetMouseButtonUp(1))
        {
            Set_Mode_Build_To_Not_Building();
        }

        //If we are over a UI element, then don't call a building method unless we are already dragging
        if (EventSystem.current.IsPointerOverGameObject() && !dragAndDropping)
        {
            return;
        }

        //Calling the Building or Placing Methods
        if (CurrentGlobalBuildingMode == GlobalBuildingMode.BuildingWall)
        {
            ConstructionController.Instance.CallingBuildingAction(MouseController.Instance.Pointer);
        }
        else if (CurrentGlobalBuildingMode == GlobalBuildingMode.PlacingFurnitures)
        {
            ConstructionController.Instance.CallingPlacingObjectAction(MouseController.Instance.Pointer);
        }
        else if (CurrentGlobalBuildingMode == GlobalBuildingMode.PlacingFloor)
        {
            ConstructionController.Instance.CallingPlacingFloorTilesAction(MouseController.Instance.Pointer, ftile);
        }
        else if (CurrentGlobalBuildingMode == GlobalBuildingMode.Bulldoze)
        {
            ConstructionController.Instance.CallingBulldozeAction(MouseController.Instance.Pointer);
        }
    }


    //Method calling the different case of building method
    public void CallingBuildingAction(MouseController pointer)
    {
        // Changing the material of the pointer
        pointer.SetMouseMaterial(ResourcesLoading.MaterialDictionnary[ResourcesLoading.MaterialNames.MousePointerCube_Blue]);

        //When clicking for the first time,
        if (Input.GetMouseButtonDown(0))
        {
            onLeftButtonPressBMethod(pointer);
            dragAndDropping = true;
        }

        //During a drap and drop
        if (dragAndDropping)
        {
            //If right click is pressed to cancel
            if (Input.GetMouseButtonDown(1))
            {
                //Drag&Drop Cancel
                dragAndDropping = false;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Finishing the drag and drop
                dragAndDropping = false;
            }
            else
            {
                //Continue Drag&Dropping
                duringDragAndDropBMethod(pointer);
            }
        }
    }

    //Method calling the different case of placing objects method
    public void CallingPlacingObjectAction(MouseController pointer)
    {

        // Changing the material of the pointer
        pointer.SetMouseMaterial(ResourcesLoading.MaterialDictionnary[ResourcesLoading.MaterialNames.MousePointerCube_Blue]);

        //When clicking for the first time,
        if (Input.GetMouseButtonDown(0))
        {
            onLeftButtonPressPMethod(pointer);
        }
        else if (Input.anyKeyDown)
        {
            OnKeyboardPressPMethod();
        }
    }

    //Method calling the different case of building method
    public void CallingPlacingFloorTilesAction(MouseController pointer, FloorTile tile)
    {
        // Changing the material of the pointer
        pointer.SetMouseMaterial(ResourcesLoading.MaterialDictionnary[ResourcesLoading.MaterialNames.MousePointerCube_Blue]);

        //When clicking for the first time,
        if (Input.GetMouseButtonDown(0))
        {
            onLeftButtonPressFMethod(pointer, tile);
            dragAndDropping = true;
        }

        //During a drap and drop
        if (dragAndDropping)
        {
            //If right click is pressed to cancel
            if (Input.GetMouseButtonDown(1))
            {
                onRightButtonPressDuringDragAndDropFMethod(pointer, tile);
                //Drag&Drop Cancel
                dragAndDropping = false;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                onLeftButtonReleaseDuringDragAndDropFMethod(pointer, tile);
                //Finishing the drag and drop
                dragAndDropping = false;
            }
            else
            {
                //Continue Drag&Dropping
                duringDragAndDropFMethod(pointer, tile);
            }
        }
    }

    //Method calling the different case of Bulldoze method
    public void CallingBulldozeAction(MouseController pointer)
    {
        // Changing the material of the pointer
        pointer.SetMouseMaterial(ResourcesLoading.MaterialDictionnary[ResourcesLoading.MaterialNames.MousePointerCube_Red]);

        //When clicking for the first time,
        if (Input.GetMouseButtonDown(0))
        {
            onLeftButtonPressBulldozeMethod(pointer);
        }
    }

    //Sets the building mode to BuildingWall;
    public void Set_Mode_Build_Wall()
    {
        CurrentBuildingMode = BuildingMode.BuildingWall;
        CurrentGlobalBuildingMode = GlobalBuildingMode.BuildingWall;
        HandleConstructionMode(CurrentBuildingMode);
    }

    //Sets the building mode to BuildingObjects
    public void Set_Mode_Place_Furnitures()
    {
        CurrentBuildingMode = BuildingMode.BuildingObjects;
        CurrentGlobalBuildingMode = GlobalBuildingMode.PlacingFurnitures;
        HandleConstructionMode(CurrentBuildingMode);
    }

    //Sets the building mode to PlacingFloorTiles
    public void Set_Mode_Placing_Floor(FloorTile tile)
    {
        CurrentBuildingMode = BuildingMode.PlacingFloor;
        CurrentGlobalBuildingMode = GlobalBuildingMode.PlacingFloor;
        HandleConstructionMode(CurrentBuildingMode);
        // Select the tile
        this.ftile = tile;
    }

    //Sets the building mode to Bulldoze
    public void Set_Mode_Build_To_Not_Building()
    {
        CurrentBuildingMode = BuildingMode.NotBuilding;
        CurrentGlobalBuildingMode = GlobalBuildingMode.NotBuilding;
        HandleConstructionMode(CurrentBuildingMode);
    }

    //Sets the building mode to NotBuilding
    public void Set_Mode_Build_To_Bulldoze()
    {
        CurrentBuildingMode = BuildingMode.Bulldoze;
        CurrentGlobalBuildingMode = GlobalBuildingMode.Bulldoze;
        HandleConstructionMode(CurrentBuildingMode);
    }


    private void HandleConstructionMode(BuildingMode bmode)
    {
        if (bmode == BuildingMode.BuildingWall)
        {
            //Setting the CreateWall Methods
            SetBuildingMethods(BuildWall.Instance);
        }
        else if (bmode == BuildingMode.BuildingObjects)
        {
            //Setting the ObjectPlacer Method
            SetPlaceObjectMethods(PlaceFurnitures.Instance);
        }
        else if (bmode == BuildingMode.PlacingFloor)
        {
            SetPlaceFloorMethods(PlaceFloor.Instance);
        }
        else if (bmode == BuildingMode.Bulldoze)
        {
            SetBulldozeMethods(Bulldoze.Instance);
        }

        //Destroy the leftovert Ghost furniture if it exists
        //PlaceObject.DestroyLeftovertGhostlyFurniture();
    }


    // Utility Method Setting all the IBuildingMethods to the BuildingGameObjects var
    private void SetBuildingMethods(IBuildingMethod buildingMethod)
    {
        onLeftButtonPressBMethod = buildingMethod.OnLeftButtonPress;
        duringDragAndDropBMethod = buildingMethod.DuringDragAndDrop;
    }

    // Utility Method Setting all the IPlaceObjectmethod to the BuildingGameObjects var
    private void SetPlaceObjectMethods(IPlaceObjectMethod placeObjectMethod)
    {
        onLeftButtonPressPMethod = placeObjectMethod.OnLeftButtonPress;
        OnKeyboardPressPMethod = placeObjectMethod.OnKeyboardPress;
    }

    // Utility Method Setting all the IplaceFloorMethod to the BuildingGameObjects var
    private void SetPlaceFloorMethods(IPlacingFloorTile placeFloorMethod)
    {
        duringDragAndDropFMethod = placeFloorMethod.DuringDragAndDrop;
        onLeftButtonPressFMethod = placeFloorMethod.OnLeftButtonPress;
        onLeftButtonReleaseDuringDragAndDropFMethod = placeFloorMethod.OnLeftButtonReleaseDuringDragAndDrop;
        onRightButtonPressDuringDragAndDropFMethod = placeFloorMethod.OnRightButtonPressDuringDragAndDrop;
    }
    // Utility Method Setting all the IplaceFloorMethod to the BuildingGameObjects var
    private void SetBulldozeMethods(IBulldozeMethod bulldozeMethod)
    {
        onLeftButtonPressBulldozeMethod = bulldozeMethod.OnLeftButtonPress;
    }


    //Utilies methods
    public static bool IsBuildingWall()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.BuildingWall);
    }

    public static bool IsPlacingFurnitures()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.PlacingFurnitures);
    }

    public static bool IsPlacingFlootTiles()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.PlacingFloor);
    }

    public static bool IsNotBuilding()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.NotBuilding);
    }
}
