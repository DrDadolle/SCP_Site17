using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Basic Behaviour of all NPCs
 * It handles :
 * -Getting and doing a job
 */
public class NPCBasicBehaviour : MonoBehaviour {

    // To move
    NavMeshAgent agent;

    //TODO : to be moved to the model for saveLoad reason ONCE JOB GETS SERIALIZED
    public Job myJob;

    // To remove ? To keep ?
    // TODO : maybe to keep for now, then to remove to use specific model for specific behaviours !
    public NPCModel theModel;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // FIXME : Should release the job when it is proven impossible to reach
        GetAJob();
        DoAJob();
    }

    protected void GetAJob()
    {
        // Get a job
        if (myJob == null)
        {
            //try to grab a job
            myJob = JobManager.jobQueue.Dequeue();
            if (myJob == null)
            {
                return;
            }
            //Set destination
            //Debug.Log("destination : " + myJob.GetTilePos());
            agent.destination = myJob.GetTilePos();

            //Set callbacks
            myJob.onJobComplete.AddListener(OnJobEnded);
            myJob.onJobCancel.AddListener(OnJobEnded);
        }
    }

    protected void DoAJob()
    {
        if (myJob != null)
        {
            //If close, stop, do job
            float _x = Mathf.Abs(gameObject.transform.position.x - myJob.GetTilePos().x);
            float _z = Mathf.Abs(gameObject.transform.position.z - myJob.GetTilePos().z);
            //if (((_x * _x) + (_z * _z)) < 0.75f )
            if (_x < .5f && _z < .5f)
            {
                agent.isStopped = true;

                //DoJob
                // FIXME : not spending one second to do it
                myJob.DoWork(Time.deltaTime);
            }
        }
    }

    protected void OnJobEnded(Job j)
    {
        // Job completed or was cancelled.

        if (j != myJob)
        {
            Debug.LogError("Character being told about job that isn't his. You forgot to unregister something.");
            return;
        }
        //Refresh the navmesh
        NavMeshController.Instance.BuildNavMesh();
        agent.ResetPath();
        myJob = null;
    }

}
