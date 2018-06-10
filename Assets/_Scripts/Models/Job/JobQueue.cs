using UnityEngine;
using System.Collections.Generic;
using System;

public class JobQueue
{
    Queue<Job> jobQueue;

    public JobQueue()
    {
        jobQueue = new Queue<Job>();
    }

    // Enqueue
    public void Enqueue(Job j)
    {
        //Check for duplicates !!!!! because of build wall issue
        if (!jobQueue.Contains(j))
                jobQueue.Enqueue(j);
    }

    //Dequeue
    public Job Dequeue()
    {
        if (jobQueue.Count == 0)
            return null;

        return jobQueue.Dequeue();
    }

    public List<Job> ConvertToJobList()
    {
        List<Job> allJobs = new List<Job>();

        while (this.jobQueue.Count > 0)
        {
            allJobs.Add(JobManager.jobQueue.Dequeue());
        }

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
