using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour {

    public NavMeshSurface WorldSurface;
    public static NavMeshController Instance;

    //Mouse cursor
    public GameObject mouseCursor;

    // Use this for initialization
    void Start () {
        Instance = this;
        // Build Once
        BuildNavMesh();
    }
	
    public IEnumerator UpdateNavMesh()
    {
        //Pause for this frame
        yield return null;

        //Update itself
        AsyncOperation op = WorldSurface.UpdateNavMesh(WorldSurface.navMeshData);
        if(op.isDone)
        {
            // update path after each back for NPCS with job !
            //Useful ? Maybe too expensive....
            foreach (var v in NPCManager.Instance.listOfNPCS.Values)
            {
                // If has a job, reupdate the destination
                if (v.GetComponent<NPCBasicBehaviour>().theModel.myJob != null)
                {
                    NavMeshAgent agent = v.GetComponent<NavMeshAgent>();
                    agent.isStopped = false;
                    agent.destination = v.GetComponent<NPCBasicBehaviour>().theModel.myJob.GetTilePos();
                }
            }
        }
    }

	public void BuildNavMesh()
    {
        WorldSurface.BuildNavMesh();
    }
}
