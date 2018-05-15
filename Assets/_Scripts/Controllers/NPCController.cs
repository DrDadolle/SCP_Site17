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
    public void PopulateNPC(NPCData data)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        //Quaternion to be changed
        GameObject go = GameObject.Instantiate(data.NPCprefab, spawnPosition, Quaternion.identity);
        go.transform.parent = gameObject.transform;

        NPCFactory.BuildNPC(data, spawnPosition, go);
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
