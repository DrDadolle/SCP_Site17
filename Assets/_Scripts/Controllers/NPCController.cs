using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  Control NPCs
 */
public class NPCController : MonoBehaviour {

    // For access purposes
    public static NPCController Instance;

    // Prefab of the NPC
    // TODO : change for a list of npcPrefab ?
    public GameObject npcPrefab;

    // On Awake
    public void Awake()
    {
        Instance = this;
    }

    // Populate a NPC (for test purpose for now)
    public void PopulateNPC(GameObject _go)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        //Quaternion to be changed
        GameObject go = GameObject.Instantiate(_go, spawnPosition, Quaternion.identity);
        go.transform.parent = gameObject.transform;
        NPCModel nPCModel = new NPCModel(Vector3.zero, 100);

        // Add a ref to the model in the behaviour script
        go.GetComponent<NPCBasicBehaviour>().theModel = nPCModel;
        NPCManager.Instance.listOfNPCS.Add(nPCModel, go);

    }

    /**
     *  Populate Npc based on the model
     *  Used by Save/Load functions
     */
    public void PopulateNPCWithModel(NPCModel model)
    {
        //Quaternion to be changed
        GameObject go = GameObject.Instantiate(npcPrefab, model.GetPos(), Quaternion.identity);
        go.transform.parent = gameObject.transform;
        go.transform.Rotate(Vector3.up * model.rotation);

        // Add a ref to the model in the behaviour script
        go.GetComponent<NPCBasicBehaviour>().theModel = model;
        NPCManager.Instance.listOfNPCS.Add(model, go);
    }
}
