using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JobQueue {

    // The  queue
    private Queue<Job> jobQueue;

    public event Action<Job> CbJobCreated;

    public JobQueue()
    {
        jobQueue = new Queue<Job>();
    }

    public void Enqueue(Job j)
    {
        jobQueue.Enqueue(j);

        //TODO : all callbacks
        if(CbJobCreated != null)
        {
            CbJobCreated(j);
        }
    }

}
