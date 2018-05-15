using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Manager of all NPCs.
 *  Might become abstract
 */
public class NPCManager : MonoBehaviour {

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
    public void UpdatePositionOfAllNPCModels()
    {
        foreach(var v in listOfNPCS)
        {
            NPCModel _model = v.Key;
            GameObject _go = v.Value;

            _model.SetPos(_go.transform.position);
            _model.rotation = _go.transform.rotation.eulerAngles.y;
        }
    }

}
