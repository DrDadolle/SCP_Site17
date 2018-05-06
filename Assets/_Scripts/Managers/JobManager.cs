using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class responsible to handle job queueS, priority etc
 */
public class JobManager : MonoBehaviour {

    public static JobQueue jobQueue;

    private bool IsCalled = false;

    // On Awake
    void Awake () {
        jobQueue = new JobQueue();
    }

}
