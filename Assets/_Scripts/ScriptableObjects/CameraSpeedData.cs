using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Camera Speed Data")]
public class CameraSpeedData : ScriptableObject {

    // Speed of camera turning when mouse moves in along an axis
    public float turnSpeed;
    // Speed of the camera going back and forth
    public float zoomSpeed;
    // Speed of the camera sliding along the plane
    public float translationSpeed; 

    // Extremum value of the zoom
    public float maxZoom;
    public float minZoom;

}
