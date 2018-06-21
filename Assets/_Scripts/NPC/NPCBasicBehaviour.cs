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
    float idleTimer = 0f;
    float idleTime;


    // To remove ? To keep ?
    // TODO : maybe to keep for now, then to remove to use specific model for specific behaviours !
    public NPCModel theModel;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isIdling = true;
        idleTime = 5f + Random.Range(-1, 8);

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
        if (isIdling)
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
            agent.speed = 1f;

            SetEndCallback();
        }
    }

    protected void DoAJob()
    {
        if (!isIdling)
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

    // Called when waiting for instructions
    // Will move randomly in a near location
    // TODO : stay in the same room ?
    // Do it every seconds ?
    protected void StartIdling()
    {


        if (isIdling)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                agent.speed = .5f;
                float x_randIdle = Random.Range(-0.5f, 0.5f) + gameObject.transform.position.x;
                float z_randIdle = Random.Range(-0.5f, 0.5f) + gameObject.transform.position.z;

                Vector3 randIdle_dest = new Vector3(x_randIdle, 0, z_randIdle);

                //Set destination
                agent.destination = randIdle_dest;


                //reset timer
                idleTimer = 0f;
                //change the next idleTime
                idleTime = 5f + Random.Range(-1, 8);

            }

        }
        //Check that we are not doing a HUGE roundabout.
    }

}
