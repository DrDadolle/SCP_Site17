﻿using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[Serializable]
public class Job
{

    // This class holds info for a queued up job, which can include
    // things like placing furniture, moving stored inventory,
    // working at a desk, and maybe even fighting enemies.

    // Position of the job
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private float z;

    float jobTime;

    [Serializable]
    public class JobCompleteEvent : UnityEvent
    {
    };

    [Serializable]
    public class JobCancelEvent : UnityEvent
    {
    };

    public JobCompleteEvent onJobComplete;

    public JobCancelEvent onJobCancel;

    /**
     * Create a job.
     * Default time to complete is 1 second.
     */ 
    public Job(Vector3Int tileposition, UnityAction cbJobComplete, float jobTime = 1f)
    {
        // FIXME : not serializable
        onJobComplete = new JobCompleteEvent();
        //onJobCancel = new JobCancelEvent();
        //onJobComplete.AddListener(cbJobComplete);
        SetTilePos(tileposition);
        this.jobTime = jobTime;
    }


    public Vector3 GetTilePos()
    {
        return new Vector3(x, y, z);
    }

    public void SetTilePos(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    // Interverting the values because of the map
    public void SetTilePos(Vector3Int pos)
    {
        this.x = pos.x + .5f;
        this.y = pos.z;
        this.z = pos.y + +.5f;
    }


    /**
     *  Do the job and invoke all the callbacks
     */
    public void DoWork(float workTime)
    {
        jobTime -= workTime;
        if (jobTime <= 0)
        {
            if (onJobComplete != null)
                onJobComplete.Invoke();
        }
    }

    /**
     *  Cancel the job and invoke all the callbacks
    */
    public void CancelJob()
    {
        if (onJobCancel != null)
            onJobCancel.Invoke();
    }

    /**
    *  Override Equals method 
    */
    public override bool Equals(object o)
    {
        if (!(o is Job))
            return false;
        else
            return this.GetTilePos().Equals(((Job)o).GetTilePos());
    }

    public override int GetHashCode()
    {
        return 1465378693 + EqualityComparer<Vector3>.Default.GetHashCode(GetTilePos());
    }
}
