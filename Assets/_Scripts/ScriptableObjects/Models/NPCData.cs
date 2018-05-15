using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableModels/NPC Data")]
public class NPCData : ScriptableObject
{

    //The type of the NPC
    public string typeOfNPC;

    //Max health of the NPC
    public float maxHealth;

    // The prefab in itself
    public GameObject NPCprefab;
}
