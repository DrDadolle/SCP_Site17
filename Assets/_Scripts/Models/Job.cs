using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class Job
{

    // This class holds info for a queued up job, which can include
    // things like placing furniture, moving stored inventory,
    // working at a desk, and maybe even fighting enemies.

    private Vector3Int tileposition;
    float jobTime;

    [Serializable]
    public class JobCompleteEvent : UnityEvent<Job>
    {
    };

    [Serializable]
    public class JobCancelEvent : UnityEvent<Job>
    {
    };


    public JobCompleteEvent onJobComplete;
    public JobCancelEvent onJobCancel;

    /**
     * Create a job.
     * Default time to complete is 1 second.
     */ 
    public Job(Vector3Int tileposition, UnityAction<Job> cbJobComplete, float jobTime = 1f)
    {
        onJobComplete = new JobCompleteEvent();
        onJobCancel = new JobCancelEvent();
        this.tileposition = tileposition;
        onJobComplete.AddListener(cbJobComplete);
        this.jobTime = jobTime;
    }

    public Vector3 GetTilePos()
    {
        return new Vector3(tileposition.x + .5f, tileposition.z, tileposition.y + .5f);
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
                onJobComplete.Invoke(this);
        }
    }

    /**
     *  Cancel the job and invoke all the callbacks
    */
    public void CancelJob()
    {
        if (onJobCancel != null)
            onJobCancel.Invoke(this);
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
