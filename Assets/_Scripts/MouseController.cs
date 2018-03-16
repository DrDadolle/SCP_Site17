using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Handling Mouse Position
 */
public class MouseController : MonoBehaviour
{

    // Last Position of the Mouse
    private Vector3 lastMousePosition;

    //Storing the building function called
    // Need to update this based on the IBuildingMethod Interface methods
    private delegate void BuildingGameObjects(MouseController pointer);
    BuildingGameObjects onLeftButtonPressMethod;
    BuildingGameObjects onLeftButtonReleaseDuringDragAndDropMethod;
    BuildingGameObjects duringDragAndDropMethod;
    BuildingGameObjects OnRightButtonPressDuringDragAndDropMethod;

    //Reference to the GameObject MousePointer
    public GameObject mousePointer;

    //Last Mouse Position
    MouseController pointer;

    //Indicate if currently drag&dropping
    private bool dragAndDropping;

    // Use this for initialization
    void Start()
    {
        pointer = GetComponent<MouseController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (ConstructionManager.getCurrentBuildingMode() == ConstructionManager.BuildingMode.BuildingWall)
        {
            //Activating the mousePointer
            mousePointer.SetActive(true);
            mousePointer.transform.position = snapPosition(getWorldPoint());

            //Setting the CreateWall Methods
            SetBuildingMethods(CreateWall.Instance);

        }
        else if (ConstructionManager.getCurrentBuildingMode() == ConstructionManager.BuildingMode.BuildingObjects)
        {
            //Activating the mousePointer
            mousePointer.SetActive(true);
            mousePointer.transform.position = snapCenterPosition(getWorldPoint());

            //Setting the ObjectPlacer Method
            SetBuildingMethods(PlaceObject.Instance);

        }
        else
        {
            mousePointer.SetActive(false);
        }


        //If we are over a UI element, then don't call a building method
        //FIXME issue dragging or when releasing
        //We just want to prevent startDrag
        //Requires changing buildings methods
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Calling the Building Methods
        if (mousePointer.activeSelf)
        {
            CallingBuildingAction(pointer);
        }

    }


    // Utility Method Setting all the IBuildingMethods to the BuildingGameObjects var
    private void SetBuildingMethods(IBuildingMethod buildingMethod)
    {
        onLeftButtonPressMethod = buildingMethod.OnLeftButtonPress;
        onLeftButtonReleaseDuringDragAndDropMethod = buildingMethod.OnLeftButtonReleaseDuringDragAndDrop;
        duringDragAndDropMethod = buildingMethod.DuringDragAndDrop;
        OnRightButtonPressDuringDragAndDropMethod = buildingMethod.OnRightButtonPressDuringDragAndDrop;
    }

    //Method calling the different case of building method
    private void CallingBuildingAction(MouseController pointer)
    {
        //When clicking for the first time,
        if (Input.GetMouseButtonDown(0))
        {
            onLeftButtonPressMethod(pointer);
            dragAndDropping = true;
        }

        //During a drap and drop
        if (dragAndDropping)
        {
            //If right click is pressed to cancel
            if (Input.GetMouseButtonDown(1))
            {
                OnRightButtonPressDuringDragAndDropMethod(pointer);
                //Drag&Drop Cancel
                dragAndDropping = false;

            }
            else if(Input.GetMouseButtonUp(0))
            {
                onLeftButtonReleaseDuringDragAndDropMethod(pointer);
                //Finishing the drag and drop
                dragAndDropping = false;
            }
            else
            {
                //Continue Drag&Dropping
                duringDragAndDropMethod(pointer);
            }
        }
    }




    public Vector3 getWorldPoint()
    {
        Camera camera = GetComponent<Camera>();
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            lastMousePosition = hit.point;
            return lastMousePosition;
        }
        //out of bound raycast
        return lastMousePosition;
    }

    /*
	 * Snap the position at the edge of the grid
	 * The y axis will be always of 0
	*/
    public Vector3 snapPosition(Vector3 original)
    {
        Vector3 snapped;
        snapped.x = Mathf.Floor(original.x + 0.5f);
        snapped.y = Mathf.Floor(0f);
        snapped.z = Mathf.Floor(original.z + 0.5f);
        return snapped;
    }

    /*
	 * Snap the position at the center of the grid
	 * The y axis will be always of 0
	*/
    public Vector3 snapCenterPosition(Vector3 original)
    {
        Vector3 snapped;
        snapped.x = Mathf.Floor(original.x) + 0.5f;
        snapped.y = Mathf.Floor(0f);
        snapped.z = Mathf.Floor(original.z) + 0.5f;
        return snapped;
    }
}

