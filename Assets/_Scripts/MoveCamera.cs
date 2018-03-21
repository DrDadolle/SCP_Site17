using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    //
    // VARIABLES
    //

    public float turnSpeed = 4.0f;      // Speed of camera turning when mouse moves in along an axis
    public float zoomSpeed = 40.0f;      // Speed of the camera going back and forth
    public float translationSpeed = 5.0f; // Speed of the camera sliding along the plane

    // Extremum value of the zoom
    public float maxZoom = 3f;
    public float minZoom = 25f;


    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private bool isRotating;    // Is the camera being rotated?



    //
    // UPDATE
    //

    void Update()
    {
        // Get the right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }


        // Disable movements on button release
        if (!Input.GetMouseButton(1)) isRotating = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            //transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera linearly along Z axis
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.transform.position.y >= maxZoom) // forward
        {
            ZoomCamera(false);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.transform.position.y <= minZoom) // backwards
        {
            ZoomCamera(true);
        }

        // Handling keyboard commands
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.left * translationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * translationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
        {
            Vector3 v = new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * (90 - transform.rotation.eulerAngles.x)), Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.x));
            transform.Translate(v * translationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Vector3 v = new Vector3(0, - Mathf.Cos(Mathf.Deg2Rad * (90 - transform.rotation.eulerAngles.x)), -  Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.x));
            transform.Translate(v * translationSpeed * Time.deltaTime);
        }

    }

    //Handling the Zoom of the camera
    void ZoomCamera(bool isZoomingout)
    {
        // Settings the vectors
        Vector3 move = zoomSpeed * transform.forward * Time.deltaTime;

        //Changing the direction of the vector
        if (isZoomingout)
            move = -move;

        //Moving the camera
        transform.Translate(move, Space.World);
    }

}