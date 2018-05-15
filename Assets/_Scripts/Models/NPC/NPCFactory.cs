using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Factory of NPC
 */
static public class NPCFactory {

    /**
     *  Create the NPC model and add it to the NPC manager !
     */
    public static void BuildNPC(NPCData data, Vector3 pos, GameObject go)
    {
        // Switch useless for now because we are using only NPCModels
        // But once the first scp will be added, it will become usefull !
        switch (data.typeOfNPC)
        {
            case "Scientist":
                NPCModel _s = new NPCModel(data, pos);
                go.GetComponent<ScientistBehaviour>().theModel = _s;
                NPCManager.Instance.listOfNPCS.Add(_s, go);        
                break;
            case "Construction_Engineer":
                NPCModel _ce = new NPCModel(data, pos);
                go.GetComponent<ConstructionEngineerBehaviour>().theModel = _ce;
                NPCManager.Instance.listOfNPCS.Add(_ce, go);
                break;
        }
    }
}
