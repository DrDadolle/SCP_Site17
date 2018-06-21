using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 *  Factory of NPC
 */
public class NPCFactory {

    /**
     *  Create the NPC model and add it to the NPC manager !
     */
    public static void BuildNPC(NPCData data, Vector3 pos, GameObject go)
    {
        // Switch useless for now because we are using only NPCModels
        // But once the first scp will be added, it will become usefull !
        switch (data.typeOfNPC)
        {
            case EnumTypes.TypeOfNPC.Researcher:
                NPCModel _s = new NPCModel(data, pos);
                go.GetComponent<ScientistBehaviour>().theModel = _s;
                NPCManager.Instance.listOfNPCS.Add(_s, go);        
                break;
            case EnumTypes.TypeOfNPC.Containement_Engineer:
                NPCModel _ce = new NPCModel(data, pos);
                go.GetComponent<ConstructionEngineerBehaviour>().theModel = _ce;
                NPCManager.Instance.listOfNPCS.Add(_ce, go);
                break;
        }
    }

    /**
    *  Create the gameobject based on the model
    *  Only used by SaveLoad Controller
    */
    public static void BuildNPCBasedOnModel(NPCModel _model)
    {
        GameObject go = null;
        switch (_model.typeOfNpc)
        {
            case EnumTypes.TypeOfNPC.Researcher:
                SetParamersFromLoad(go, _model, ResourcesLoading.NPCPrefabNames.Scientist);
                break;
            case EnumTypes.TypeOfNPC.Containement_Engineer:
                SetParamersFromLoad(go, _model, ResourcesLoading.NPCPrefabNames.Construction_Engineer);
                break;
        }
    }

    private static void SetParamersFromLoad(GameObject go, NPCModel _model, ResourcesLoading.NPCPrefabNames nameOfNpcPrefabNames)
    {
        go = GameObject.Instantiate(ResourcesLoading.NPCPrefabDic[nameOfNpcPrefabNames], _model.GetPos(), Quaternion.identity);
        go.transform.Rotate(Vector3.up * _model.rotation);
        NPCBasicBehaviour Behav = go.GetComponent<NPCBasicBehaviour>();
        Behav.theModel = _model;
        NPCManager.Instance.listOfNPCS.Add(_model, go);

        // Convert Back the job from jobLite
        if (_model.myJobLite != null)
        {
            _model.myJob = new Job(_model.myJobLite);
            go.GetComponent<NavMeshAgent>().destination = _model.myJob.GetTilePos();
            Behav.SetEndCallback();
        }
    }

}
