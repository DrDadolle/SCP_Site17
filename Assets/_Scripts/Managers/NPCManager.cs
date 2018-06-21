using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Manager of all NPCs.
 *  Might become abstract
 */
public class NPCManager : MonoBehaviour
{

    public static NPCManager Instance;

    // FIXME : change to the model of npcs
    public Dictionary<NPCModel, GameObject> listOfNPCS = new Dictionary<NPCModel, GameObject>();

    // On Awake
    private void Awake()
    {
        Instance = this;
    }

    /**
     *  Update the position of all NPCMOdel
     */
    public void UpdatePositionAndJobLiteOfAllNPCModels()
    {
        foreach (var v in listOfNPCS)
        {
            NPCModel _model = v.Key;
            GameObject _go = v.Value;

            _model.SetPos(_go.transform.position);
            _model.rotation = _go.transform.rotation.eulerAngles.y;

            //No job
            if(_model.myJob != null)
            {
                _model.myJobLite = new JobLite(_model.myJob);
            }
            
        }
    }

    /**
     * Get an NPCModel idling, with max job priority over the task and the closest
     * TODO : optimize this, probably expensive !
     * Optimize this by creating an "idling" list of NPCs which is dynamic ?
     * TODO : T OBE DELETED ?
     */
     public NPCModel GetNPCBasedOnJobPriorityAndPosition(Vector3 pos, EnumTypes.JobMacroType jobMacroType)
    {
        NPCModel _tmpret = null;
        NPCModel ret = null;
        int _tmpPriority = -1;
        Vector3 distance = new Vector3(-200000, -200000, -200000);

        EnumTypes.TypeOfNPC _npctype;
        foreach(var npc in listOfNPCS)
        {
            // if not jobless
            if (!npc.Value.GetComponent<NPCBasicBehaviour>().isIdling)
                continue;
            _npctype = npc.Key.typeOfNpc;
            //check that the NPC types correspond to a relevant job
            // UGLY SWITCH
            switch (jobMacroType)
            {
                case EnumTypes.JobMacroType.Building:
                    if (_npctype == EnumTypes.TypeOfNPC.Containement_Engineer ||
                             _npctype == EnumTypes.TypeOfNPC.D_Class ||
                            _npctype == EnumTypes.TypeOfNPC.Researcher)
                        _tmpret = npc.Key;
                    break;
                case EnumTypes.JobMacroType.Research:
                    if (_npctype == EnumTypes.TypeOfNPC.Researcher)
                        _tmpret = npc.Key;
                    break;
            }

            // if no jobless npc with relevant job priorities,  bail out
            if (_tmpret == null)
                continue;

            // get the top most job priority npcs, then the closest
            foreach (var v in _tmpret.JobPriorities)
            {
                //Get the right jobMacroType
                if (v.JobMacroType == jobMacroType)
                {
                    // get most priority jo
                    if(v.priority > _tmpPriority)
                    {
                        distance = npc.Value.transform.position;
                        ret = _tmpret;
                        _tmpPriority = v.priority;
                    }
                    else if (v.priority == _tmpPriority)
                    {
                        //Get closest npc
                        //FIXME
                        // ISSUE IS : it is distance based ONLY on "vol d'oiseau" not true pathfinding distance
                        if(Vector3.Distance(npc.Value.transform.position, pos) < Vector3.Distance(distance, pos))
                        {
                            distance = npc.Value.transform.position;
                            ret = _tmpret;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }

        return ret;
    }

}
