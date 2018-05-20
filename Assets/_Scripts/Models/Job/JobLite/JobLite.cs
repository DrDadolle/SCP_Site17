using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Lite job ONLY FOR SAVE/LOAD purposes
 */
[System.Serializable]
public abstract class JobLite {

    // Position
    [SerializeField]
    protected float x;
    [SerializeField]
    protected float y;
    [SerializeField]
    protected float z;

    // Who is doing the job
    // TODO : give unique name to NPCS
    public string NameOfNPC;

    // Time spent on the task
    public float jobTime;

    // Type of the job, refers to job type name in JobActions
    public string JobType;

    public JobLite(Vector3 position, string nameOfWorker, string typeOfJob, float jobTime)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
        this.NameOfNPC = nameOfWorker;
        this.JobType = typeOfJob;
        this.jobTime = jobTime;
    }

    public JobLite(Job j)
    {
        this.x = j.GetTilePos().x;
        this.y = j.GetTilePos().y;
        this.z = j.GetTilePos().z;
        this.NameOfNPC = null;
        this.JobType = j.TypeOfJob;
        this.jobTime = j.JobTime;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}
