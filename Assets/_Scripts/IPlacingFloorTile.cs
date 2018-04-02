using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacingFloorTile {

    // OnLeftButtonPress
    // Used for placing object
    void OnLeftButtonPress(MouseController pointer, FloorTile tile);

    // DuringDragAnDrop
    // Method called during each frame of a drag and drop
    void DuringDragAndDrop(MouseController pointer, FloorTile tile);

    // OnLeftButtonReleaseDuringDragAndDrop
    // Method called at the end of the drag and drop
    void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer, FloorTile tile);

    // OnRightButtonPressDuringDragAndDrop
    // Method called when canceling a drag and drop
    void OnRightButtonPressDuringDragAndDrop(MouseController pointer, FloorTile tile);

}
