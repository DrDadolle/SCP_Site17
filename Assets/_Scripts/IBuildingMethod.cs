using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Interface of ALL the buildings functions
public interface IBuildingMethod
{
    // OnUpdateWhenTileIsChanged
    // Used for placement of objects
    void OnUpdateWhenTileIsChanged(TileBase tile);

    // OnLeftButtonPress
    // Used for placing object or initiating drag and dropping
    void OnLeftButtonPress(TileBase tile);

    // OnLeftButtonReleaseDuringDragAndDrop
    // Used to finish a drag and drop of the left mouse button
    void OnLeftButtonReleaseDuringDragAndDrop(TileBase tile);

    // DuringDragAnDrop
    // Method called during each frame of a drag and drop
    void DuringDragAndDrop(TileBase tile);

    // OnRightButtonPress
    // Used for canceling stuff
    void OnRightButtonPressDuringDragAndDrop(TileBase tile);

    // OnKeyboardPress
    //Use to handle shortcuts, rotation and other behaviours
    void OnKeyboardPress(TileBase tile);

}
