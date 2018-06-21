using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class responsible to handle job queueS, priority etc
 */
public class JobManager : MonoBehaviour {

    public static JobQueue jobQueue;

    // For access purposes
    public static JobManager Instance;

    // On Awake
    void Awake () {
        jobQueue = new JobQueue();
        Instance = this;
    }

    //On Update
    private void Update()
    {
        // Optimize this using callbacks on "job end" ?
        /*while(jobQueue.buildingJobs.Count > 0)
        {
            Job j = jobQueue.GetForMacroType(EnumTypes.JobMacroType.Building);
            NPCModel npc = NPCManager.Instance.GetNPCBasedOnJobPriorityAndPosition(j.GetPosition(), EnumTypes.JobMacroType.Building);

            // no available npc = bailout
            if (npc == null)
                break;

            //Assign the job and remove it from 
            NPCManager.Instance.listOfNPCS[npc].GetComponent<NPCBasicBehaviour>().AssignJob(j);
            jobQueue.RemoveFromListJob(j);

        }*/
        // If not, go next list !

        
    }

}
