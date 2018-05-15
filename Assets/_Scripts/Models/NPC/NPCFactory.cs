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

    /**
    *  Create the gameobject based on the model
    */
    public static void BuildNPCBasedOnModel(NPCModel _model)
    {
        GameObject go = null;
        switch (_model.typeOfNpc)
        {
            case "Scientist":
                go = GameObject.Instantiate(ResourcesLoading.NPCPrefabDic[ResourcesLoading.NPCPrefabNames.Scientist], _model.GetPos(), Quaternion.identity);
                go.transform.Rotate(Vector3.up * _model.rotation);
                go.GetComponent<ScientistBehaviour>().theModel = _model;
                NPCManager.Instance.listOfNPCS.Add(_model, go);
                break;
            case "Construction_Engineer":
                go = GameObject.Instantiate(ResourcesLoading.NPCPrefabDic[ResourcesLoading.NPCPrefabNames.Construction_Engineer], _model.GetPos(), Quaternion.identity);
                go.transform.Rotate(Vector3.up * _model.rotation);
                go.GetComponent<ConstructionEngineerBehaviour>().theModel = _model;
                NPCManager.Instance.listOfNPCS.Add(_model, go);
                break;
        }
    }
}
