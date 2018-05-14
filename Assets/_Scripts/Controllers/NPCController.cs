using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  Control NPCs
 */
public class NPCController : MonoBehaviour {

    public GameObject npcPrefab;

    // Populate a NPC (for test purpose for now)
    public void PopulateNPC()
    {
        //Quaternion to be changed
        GameObject go = GameObject.Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);
        NPCManager.Instance.listOfNPCS.Add(go);
    }

    //TODO : create Seriazebla NPCModel
}
