using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface of ALL the buildings Walls functions
public interface IPlaceObjectMethod
{
    // OnLeftButtonPress
    // Used for placing object
    void OnLeftButtonPress(MouseController pointer);

    // OnUpdate
    // Used to display a position at every update
    void OnUpdate(MouseController pointer);

    // OnKeyboardPress
    //Use to handle shortcuts and other behaviours
    void OnKeyboardPress();

}
