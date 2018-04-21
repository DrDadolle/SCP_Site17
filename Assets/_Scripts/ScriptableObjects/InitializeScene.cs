using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Class setting everything in the scene at first
 *  It is a ScriptableObject as everything inside it will be used by everything on the scene
 */

[CreateAssetMenu(menuName = "ScriptableObjects/InitializeScene")]
public class InitializeScene : ScriptableObject{

    //TODO : Will be replaced with a dedicate class
    // for managing job queues (plural!)
	public JobQueue jobQueue;


    private void OnEnable()
    {
        jobQueue = new JobQueue();
    }
}
