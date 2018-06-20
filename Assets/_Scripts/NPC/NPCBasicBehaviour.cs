using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Basic Behaviour of all NPCs
 * It handles :
 * -Getting and doing a job
 */
public abstract class NPCBasicBehaviour : MonoBehaviour {

    // To move
    NavMeshAgent agent;

    // Idling 
    public bool isIdling;

    // To remove ? To keep ?
    // TODO : maybe to keep for now, then to remove to use specific model for specific behaviours !
    public NPCModel theModel;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isIdling = true;
        
    }

    /*private void Update()
    {
        // FIXME : Should release the job when it is proven impossible to reach
        GetAJob();
        DoAJob();
    }*/

    // Get job from an unique jobqueue
    protected void GetAJobFromJobQueue(JobQueue queue)
    {
        // Get a job
        if (theModel.myJob == null)
        {
            //try to grab a job
            theModel.myJob = queue.DequeueForNPC(gameObject.transform.position, "Building");
            if (theModel.myJob == null)
            {
                return;
            }
            isIdling = false;

            //Set destination
            agent.destination = theModel.myJob.GetTilePos();

            SetEndCallback();
        }
    }

    protected void DoAJob()
    {
        if (theModel.myJob != null)
        {
            //If close, stop, do job
            float _x = Mathf.Abs(gameObject.transform.position.x - theModel.myJob.GetTilePos().x);
            float _z = Mathf.Abs(gameObject.transform.position.z - theModel.myJob.GetTilePos().z);
            if (_x < .5f && _z < .5f)
            {
                //Remove as we will be using the stoppingdistance property
                agent.isStopped = true;
                //DoJob
                theModel.myJob.DoWork(Time.deltaTime);
            }
        }
    }

    protected void OnJobEnded()
    {
        //Refresh the navmesh
        StartCoroutine(NavMeshController.Instance.UpdateNavMesh());
        agent.ResetPath();
        theModel.myJob = null;
        isIdling = true;
    }

    public void SetEndCallback()
    {
        //Set callbacks
        theModel.myJob.onJobComplete.AddListener(OnJobEnded);
        theModel.myJob.onJobCancel.AddListener(OnJobEnded);
    }

}
