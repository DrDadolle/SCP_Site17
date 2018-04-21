using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Mouse Pointer")]
public class MousePointer : ScriptableObject {

    // The Pointer itself
    public GameObject go;

    //Get the main camera
    private Camera mainCamera;

    //Layer mask
    private int layer_mask;

    // max distance for the raycast
    private float maxDistance = Mathf.Infinity;

    //Last Mouse Position
    private Vector3 lastMousePosition;

    // On enable
    private void OnEnable()
    {
        mainCamera = Camera.main;
        layer_mask = LayerMask.GetMask("Terrain");
        go = GameObject.FindGameObjectsWithTag("MousePointer")[0];
    }


    /**
     * Get the world point on a raycast from the camera to the mouse position
    */
    public Vector3 GetWorldPoint()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, layer_mask))
        {
            lastMousePosition = hit.point;
            return lastMousePosition;
        }
        //out of bound raycast
        return lastMousePosition;
    }

    //Activating the mousePointer for Furnitures
    public void SetMouseToFurnitureGrid()
    {
        go.SetActive(true);
        go.transform.position = UtilitiesMethod.snapCenterPosition(GetWorldPoint());
        go.transform.position += Vector3.up * 0.5f;
    }
}
