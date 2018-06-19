using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Lite job ONLY FOR SAVE/LOAD purposes
 */
[System.Serializable]
public class JobLite {

    // This class holds info for a queued up job, which can include
    // things like placing furniture, moving stored inventory,
    // working at a desk, and maybe even fighting enemies.

    // Position of the job

    // Position
    [SerializeField]
    protected float x;
    [SerializeField]
    protected float y;
    [SerializeField]
    protected float z;

    // Time spent on the task
    public float jobTime;

    // General type of the job, like "construction job" or such.
    // Used to determine the queueJob it belongs to.
    public string jobMacroType;

    // Name of the job, refers to job type name in JobActions. It's the ID on the jobType
    public string jobName;

    // Parent constructor used by Job Class
    public JobLite(string typeOfJob, float jobTime, string jobMacroType)
    {
        this.jobName = typeOfJob;
        this.jobTime = jobTime;
        this.jobMacroType = jobMacroType;
    }

    //Convert Job to JobLite !
    public JobLite(Job j)
    {
        this.x = j.GetPosition().x;
        this.y = j.GetPosition().y;
        this.z = j.GetPosition().z;
        this.jobTime = j.jobTime;
        this.jobName = j.jobName;
        this.jobMacroType = j.jobMacroType;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}
