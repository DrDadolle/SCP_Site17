using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour {

    public NavMeshSurface[] surfaces;
    public static NavMeshController Instance;

    //Mouse cursor
    public GameObject mouseCursor;

    // Use this for initialization
    void Start () {
        Instance = this;
        BuildNavMesh();
    }
	
    
	public void BuildNavMesh()
    {
        //before baking, diseable  the mesh renderer of the mouse ?
        // FIXME : change this workaround
        mouseCursor.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < surfaces.Length; i++)
        {
            // TODO It is building for all objects, maybe we do not want that
            surfaces[i].BuildNavMesh();
        }
        //After baking, diseable  the mesh renderer of the mouse ?
        mouseCursor.GetComponent<MeshRenderer>().enabled = true;

        // update path after each back for NPCS with job !
        foreach(var v in NPCManager.Instance.listOfNPCS.Values)
        {
            // If has a job, reupdate the destination
            if(v.GetComponent<NPCBasicBehaviour>().theModel.myJob != null)
            {
                NavMeshAgent agent = v.GetComponent<NavMeshAgent>();
                agent.isStopped = false;
                agent.destination = v.GetComponent<NPCBasicBehaviour>().theModel.myJob.GetTilePos();
            }
        }

    }
}
