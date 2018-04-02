using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureBehaviour : MonoBehaviour {

    // Number of trigger actually entered 
    private int numberOfTrigger = 0;

    //Handle Trigger Enter
    void OnTriggerEnter(Collider other)
    {
        numberOfTrigger++;
        UtilitiesMethod.ChangeMaterialOfRecChildGameObject(gameObject, ResourcesLoading.ghostly_red);
    }

    //Handle Trigger Exit
    void OnTriggerExit(Collider other)
    {
        numberOfTrigger--;
        if (numberOfTrigger == 0)
        {
            UtilitiesMethod.ChangeMaterialOfRecChildGameObject(gameObject, ResourcesLoading.ghostly_blue);
        }
    }
}
