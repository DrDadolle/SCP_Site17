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

    public void Enqueue(Job j)
    {
        //Check for duplicates !!!!! because of build wall issue
        if (!jobQueue.Contains(j))
                jobQueue.Enqueue(j);

    }

    public Job Dequeue()
    {
        if (jobQueue.Count == 0)
            return null;

        return jobQueue.Dequeue();
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
