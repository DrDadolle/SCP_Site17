using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionEngineerBehaviour : NPCBasicBehaviour
{

    private void Update()
    {
        // FIXME : Should release the job when it is proven impossible to reach
        //For now we specify the queue they want to use.
        // Then we should use a queue list of "jobQueueVariable" objects ?
        GetAJobFromJobQueue(JobManager.jobQueue);
        DoAJob();
    }

}

