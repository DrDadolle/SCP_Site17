using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface of ALL the buildings functions
public interface IBuildingMethod
{
    // OnLeftButtonPress
    // Used for placing object or initiating drag and dropping
    void OnLeftButtonPress(MouseController pointer);

    // OnLeftButtonReleaseDuringDragAndDrop
    // Used to finish a drag and drop of the left mouse button
    void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer);

    // DuringDragAnDrop
    // Method called during each frame of a drag and drop
    void DuringDragAndDrop(MouseController pointer);

    // OnRightButtonPress
    // Used for canceling stuff
    void OnRightButtonPressDuringDragAndDrop(MouseController pointer);

}
