using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructionManager : MonoBehaviour {

	public static BuildingMode currentBuildingMode {
        get; private set;
    }
    public static GlobalBuildingMode CurrentGlobalBuildingMode {
        get; private set;
    }

	public enum BuildingMode {
		NotBuilding,
		BuildingWall,
		BuildingDoubleWalls,
		BuildingObjects
	};

    public enum GlobalBuildingMode
    {
        NotBuilding,
        BuildingWall,
        PlacingFurnitures
    };


    //Storing the building function called
    // Need to update this based on the IBuildingMethod Interface methods
    private delegate void BuildingGameObjects(MouseController pointer);
    BuildingGameObjects onLeftButtonPressBMethod;
    BuildingGameObjects onLeftButtonReleaseDuringDragAndDropBMethod;
    BuildingGameObjects duringDragAndDropBMethod;
    BuildingGameObjects onRightButtonPressDuringDragAndDropBMethod;

    //Storing the place object function called
    // Need to update this based on the IPlaceObjectmethod Interface methods
    private delegate void PlacingGameObjects(MouseController pointer);
    PlacingGameObjects onLeftButtonPressPMethod;
    PlacingGameObjects onUpdatePMethod;

    private delegate void PlacingGamesObjects_withoutPointer();
    PlacingGamesObjects_withoutPointer OnKeyboardPressPMethod;
    
    //Indicate if currently drag&dropping
    private bool dragAndDropping;

    // used for access purposes
    public static ConstructionManager Instance;


    //On Awake
    void Awake () {
        Instance = this;
		currentBuildingMode = BuildingMode.NotBuilding;
        CurrentGlobalBuildingMode = GlobalBuildingMode.NotBuilding;
	}
	
	// Update is called once per frame
	void Update () {

        //Change the mode of construction
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            currentBuildingMode = BuildingMode.NotBuilding;
            CurrentGlobalBuildingMode = GlobalBuildingMode.NotBuilding;
        }

        //If we are over a UI element, then don't call a building method
        //FIXME issue dragging or when releasing
        //We just want to prevent startDrag
        //Requires changing buildings methods
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Calling the Building or Placing Methods
        if (CurrentGlobalBuildingMode == GlobalBuildingMode.BuildingWall)
        {
            ConstructionManager.Instance.CallingBuildingAction(MouseController.Instance.Pointer);
        }
        else if (CurrentGlobalBuildingMode == GlobalBuildingMode.PlacingFurnitures)
        {
            ConstructionManager.Instance.CallingPlacingObjectAction(MouseController.Instance.Pointer);
        }
    }

    //Sets the building mode to BuildingWall;
    public void Set_Mode_Build_Wall()
    {
        currentBuildingMode = BuildingMode.BuildingWall;
        CurrentGlobalBuildingMode = GlobalBuildingMode.BuildingWall;
        HandleConstructionMode(currentBuildingMode);
    }

    //Sets the building mode to BuildingObjects
    public void Set_Mode_Place_Furnitures()
    {
        currentBuildingMode = BuildingMode.BuildingObjects;
        CurrentGlobalBuildingMode = GlobalBuildingMode.PlacingFurnitures;
        HandleConstructionMode(currentBuildingMode);
    }

	//Sets the building mode to BuildingDoubleWalls
	public void Set_Mode_Build_Double_Walls()
	{
		currentBuildingMode = BuildingMode.BuildingDoubleWalls;
        CurrentGlobalBuildingMode = GlobalBuildingMode.BuildingWall;
        HandleConstructionMode(currentBuildingMode);

    }


    private void HandleConstructionMode(BuildingMode bmode)
    {
        if (bmode == BuildingMode.BuildingWall)
        {
            //Setting the CreateWall Methods
            SetBuildingMethods(CreateWall.Instance);
        }
        else if (bmode == BuildingMode.BuildingObjects)
        {
            //Setting the ObjectPlacer Method
            SetPlaceObjectMethods(PlaceObject.Instance);
        }
        else if (bmode == BuildingMode.BuildingDoubleWalls)
        {
            //Setting the CreateDoubleWalls Methods
            SetBuildingMethods(CreateDoubleWalls.InstanceDoubleWall);
        }

        //Destroy the leftovert Ghost furniture if it exists
        PlaceObject.DestroyLeftovertGhostlyFurniture();
    }


    // Utility Method Setting all the IBuildingMethods to the BuildingGameObjects var
    private void SetBuildingMethods(IBuildingMethod buildingMethod)
    {
        onLeftButtonPressBMethod = buildingMethod.OnLeftButtonPress;
        onLeftButtonReleaseDuringDragAndDropBMethod = buildingMethod.OnLeftButtonReleaseDuringDragAndDrop;
        duringDragAndDropBMethod = buildingMethod.DuringDragAndDrop;
        onRightButtonPressDuringDragAndDropBMethod = buildingMethod.OnRightButtonPressDuringDragAndDrop;
    }

    // Utility Method Setting all the IPlaceObjectmethod to the BuildingGameObjects var
    private void SetPlaceObjectMethods(IPlaceObjectMethod placeObjectMethod)
    {
        onLeftButtonPressPMethod = placeObjectMethod.OnLeftButtonPress;
        onUpdatePMethod = placeObjectMethod.OnUpdate;
        OnKeyboardPressPMethod = placeObjectMethod.OnKeyboardPress;
    }


    //Method calling the different case of building method
    public void CallingBuildingAction(MouseController pointer)
    {
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
                onRightButtonPressDuringDragAndDropBMethod(pointer);
                //Drag&Drop Cancel
                dragAndDropping = false;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                onLeftButtonReleaseDuringDragAndDropBMethod(pointer);
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

        //On Update
        onUpdatePMethod(pointer);

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

    //Utilies methods
    public static bool IsBuildingWall()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.BuildingWall);
    }

    public static bool IsPlacingFurnitures()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.PlacingFurnitures);
    }

    public static bool IsNotBuilding()
    {
        return CurrentGlobalBuildingMode.Equals(GlobalBuildingMode.NotBuilding);
    }
}
