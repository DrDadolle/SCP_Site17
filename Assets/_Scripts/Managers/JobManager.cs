using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class responsible to handle job queueS, priority etc
 */
public class JobManager : MonoBehaviour {

    public static JobQueue jobQueue;

    private bool IsCalled = false;

    // For access purposes
    public static JobManager Instance;

    // On Awake
    void Awake () {
        jobQueue = new JobQueue();
        Instance = this;
    }

}
