using UnityEngine;
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

    public float JobTime
    { get; private set; }

    public string TypeOfJob
    { get; private set; }

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
    public Job(Vector3Int tileposition, UnityAction cbJobComplete, float jobTime = 1f, string TypeOfJob = "")
    {
        onJobComplete = new JobCompleteEvent();
        onJobCancel = new JobCancelEvent();
        onJobComplete.AddListener(cbJobComplete);
        SetTilePos(tileposition);
        this.JobTime = jobTime;
        this.TypeOfJob = TypeOfJob;
    }

    /**
     *  Create job from job lite !
     *  On Load
     */
     public Job(JobLite j)
    {
        //Special Switch Case to add the callbacks !
        onJobComplete = new JobCompleteEvent();
        onJobCancel = new JobCancelEvent();

        SetTilePos(j.GetPosition());
        this.JobTime = j.jobTime;
        this.TypeOfJob = j.JobType;

        //Fixing the job position issue when creating the callbacks, so we target the correct position in the managers
        // We intervert back x and y field, and remove the 0.5f used to center the position of the job !
        // TODO : replace the getPostion by GetCenterOfTile and GetPosition to avoid having to much of these weird conversion
        Vector3Int correctedPosition = Vector3Int.CeilToInt(new Vector3(GetTilePos().x - .5f, GetTilePos().z - .5f, GetTilePos().y));


        //SWITCH CASE to determine the job type
        switch (j.JobType) {
            case "Wall":
                onJobComplete.AddListener(JobActions.BuildWallJob(WorldManager.Instance.world, correctedPosition));
                break;
            case "Furniture":
                onJobComplete.AddListener(JobActions.BuildFurnitureJob(WorldManager.Instance.world, correctedPosition));
                break;
            case "Floor":
                onJobComplete.AddListener(JobActions.BuildFloorJob(WorldManager.Instance.world, correctedPosition));
                break;
        }

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
        JobTime -= workTime;
        if (JobTime <= 0)
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
