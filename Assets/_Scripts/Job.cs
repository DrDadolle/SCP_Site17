using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 *  This class holds information of a queued job like placing floor, walls, etc...
 */
public class Job {

    // Position of the tile on the tilemap
    Vector3Int tilePos;

    //time to finish the job
    float jobTime = 1f;

    //Callbacks
    public event Action<Job> CallbackJobComplete;
    public event Action<Job> CallbackJobCancel;

    /**
     *  Constructor
     */
    public Job(Vector3Int tilePos, Action<Job> cbJobComplete, float jobTime = 1f)
    {
        this.tilePos = tilePos;
        this.jobTime = jobTime;
        this.CallbackJobComplete += cbJobComplete;
    }

    public void DoWork(float workTime)
    {
        jobTime -= workTime;
        if(jobTime <= 0)
        {
            if(CallbackJobComplete != null)
                CallbackJobComplete(this);
        }
    }

    public void CancelJob()
    {
        if (CallbackJobCancel != null)
            CallbackJobCancel(this);
    }


}
