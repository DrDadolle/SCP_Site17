using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public static NPCManager Instance;

    // FIXME : change to the model of npcs
    public List<GameObject> listOfNPCS = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

}
