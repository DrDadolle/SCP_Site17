using UnityEngine;
using System.Collections.Generic;
using System;

/**
 * TODO : To be reworked asap !
 * Required features :
 * -Priority system
 * -Assign closest job
 * -Type of jobs : Building, etc...
 * - Need a special class to handle emergency ? Like mass notification ?
 */ 
public class JobQueue
{
    // List of building Jobs
    public List<Job> buildingJobs;

    // Science Jobs
    public List<Job> scienceJobs;

    public JobQueue()
    {

        buildingJobs = new List<Job>();
        scienceJobs = new List<Job>();
    }

    // Enqueue
    public void Enqueue(Job j)
    {
        switch (j.jobMacroType)
        {
            case EnumTypes.JobMacroType.Building:
                if (!buildingJobs.Contains(j))
                    buildingJobs.Add(j);
                break;
            case EnumTypes.JobMacroType.Research:
                if (!scienceJobs.Contains(j))
                    scienceJobs.Add(j);
                break;
        }

    }

    //Dequeue
    public Job DequeueForNPC(Vector3 NPCPosition, EnumTypes.JobMacroType jobMacroType)
    {
        switch (jobMacroType)
        {
            case EnumTypes.JobMacroType.Building:
                if(buildingJobs.Count != 0)
                {
                    return GetClosestJobFromNPC(buildingJobs, NPCPosition);
                }
                break;
            case EnumTypes.JobMacroType.Research:
                if (scienceJobs.Count != 0)
                {
                    return GetClosestJobFromNPC(scienceJobs, NPCPosition);
                }
                break;
            default:
                return null;
        }
        return null;
    }

    /**
     *  Get the first job based on macrotype.
     *  DOES NOT REMOVE IT FROM THE QUEUE
     */
    public Job GetForMacroType(EnumTypes.JobMacroType jobMacroType)
    {
        Job j;
        switch (jobMacroType)
        {
            case EnumTypes.JobMacroType.Building:
                j = buildingJobs[0];
                return j;
            case EnumTypes.JobMacroType.Research:
                j = scienceJobs[0];
                return j;
            default:
                return null;
        }
    }

    /**
     *  Remove the job from the list
     */
    public bool RemoveFromListJob(Job j)
    {
        switch(j.jobMacroType)
        {
            case EnumTypes.JobMacroType.Building:
                return buildingJobs.Remove(j);
            case EnumTypes.JobMacroType.Research:
                return scienceJobs.Remove(j);
            default:
                return false;
        }
    }



    private Job GetClosestJobFromNPC(List<Job> theList, Vector3 pos)
    {
        Job closestJob = theList[0];

        foreach (var j in theList)
        {
            if (Vector3.Distance(j.GetTilePos(), pos) < Vector3.Distance(closestJob.GetTilePos(), pos))
                closestJob = j;

        }

        // Delete from the list the taken job
        theList.Remove(closestJob);

        return closestJob;
    }

    public List<Job> ConvertToJobList()
    {
        List<Job> allJobs = new List<Job>();

        allJobs.AddRange(buildingJobs);
        allJobs.AddRange(scienceJobs);
         
        return allJobs;
    }

    public void ClearAll()
    {
        buildingJobs.Clear();
        scienceJobs.Clear();
    }

}
