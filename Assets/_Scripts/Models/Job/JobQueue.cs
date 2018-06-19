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
    List<Job> buildingJobs;

    // Science Jobs
    List<Job> scienceJobs;

    // Deprecated...
    Queue<Job> jobQueue;

    public JobQueue()
    {

        buildingJobs = new List<Job>();
        scienceJobs = new List<Job>();

        // Deprecated
        jobQueue = new Queue<Job>();
    }

    // Enqueue
    public void Enqueue(Job j)
    {
        //WIP to be done, with list<Job> !!
        switch (j.jobMacroType)
        {
            case "Building":
                if (!buildingJobs.Contains(j))
                    buildingJobs.Add(j);
                break;
            case "Research":
                if (!scienceJobs.Contains(j))
                    scienceJobs.Add(j);
                break;
        }

    }

    //Dequeue
    public Job DequeueForNPC(Vector3 NPCPosition, string jobMacroType)
    {
        switch (jobMacroType)
        {
            case "Building":
                if(buildingJobs.Count != 0)
                {
                    return GetClosestJobFromNPC(buildingJobs, NPCPosition);
                }
                break;
            case "Research":
                if (buildingJobs.Count != 0)
                {
                    return GetClosestJobFromNPC(scienceJobs, NPCPosition);
                }
                break;
            default:
                return null;
        }
        return null;
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

    public int GetJobCount()
    {
        return jobQueue.Count;
    }

    public void ClearAll()
    {
        jobQueue.Clear();
    }

}
