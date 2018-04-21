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

    //Reference to the GameObjectVariable MousePointer
    public MousePointer mousePointer;

    // ScriptableObject of the building Mode
    public BooleanVariables areWeBuilding;

    // It is called on every frame
    private void Update()
    {
        //Refresh the pointer Position
        if(!areWeBuilding.theboolean)
        {
            //Fix this call
            mousePointer.go.SetActive(false);
        }
        else
        {
            //Activating the mousePointer
            mousePointer.SetMouseToFurnitureGrid();
        }
    }
}

