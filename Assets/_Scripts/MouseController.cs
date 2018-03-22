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

    //Reference to the GameObject MousePointer
    public GameObject mousePointer;

    //Last Mouse Position
    public MouseController Pointer {
        get; private set;
    }

    // Used for access purpose
    public static MouseController Instance;

    // Use this for initialization
    void Start()
    {
        Pointer = GetComponent<MouseController>();

    }

    // On Awake
    private void Awake()
    {
        Instance = this;
    }

    // It is called on every frame
    private void Update()
    {
        //Refresh the pointer Position
        if (ConstructionManager.IsBuildingWall())
        {
            //Activating the mousePointer
            SetMouseToWallGrid();
        }
        else if(ConstructionManager.IsPlacingFurnitures())
        {
            //Activating the mousePointer
            SetMouseToFurnitureGrid();
        }
        else if(ConstructionManager.IsNotBuilding())
        {
            DiseableMousePointer();
        }


    }


    //Activating the mousePointer for Walls
    public void SetMouseToWallGrid()
    {
        mousePointer.SetActive(true);
        mousePointer.transform.position = snapPosition(getWorldPoint());
    }

    //Activating the mousePointer for Furnitures
    public void SetMouseToFurnitureGrid()
    {
        mousePointer.SetActive(true);
        mousePointer.transform.position = snapCenterPosition(getWorldPoint());
    }

    //Desactivate the mouse Pointer
    public void DiseableMousePointer()
    {
        mousePointer.SetActive(false);
    }

    //Return the state of the mouse Pointer
    public static bool IsMousePointerActive()
    {
        return Instance.mousePointer.activeSelf;
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

