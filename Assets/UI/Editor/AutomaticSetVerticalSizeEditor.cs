using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Customise the editor for the AutomaticSetVerticalSize object in the GUI
[CustomEditor(typeof(AutomaticSetVerticalSize))]
public class AutomaticSetVerticalSizeEditor : Editor {

    public override void OnInspectorGUI()
    {
        //Draw the basic inspector
        DrawDefaultInspector();

        // true means that the button got pushed
       if( GUILayout.Button("Recalc Size of Menu"))
        {
            ((AutomaticSetVerticalSize)target).AdjustSize();
        }
    }
}
