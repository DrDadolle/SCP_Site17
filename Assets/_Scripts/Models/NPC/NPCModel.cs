﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Parent class of all npc models
 * TODO : add abstract to it !
 */
[Serializable]
public class NPCModel {

    public EnumTypes.TypeOfNPC typeOfNpc;

    // Life of the npc
    public float hp;

    // Position of the npc
    [SerializeField]
    private float x;
    [SerializeField]
    private float y;
    [SerializeField]
    private float z;

    public float rotation;

    // Its job
    [NonSerialized]
    public Job myJob;

    // JobLite only for saveLoad
    public JobLite myJobLite;

    public List<NPCData.JobPriority> JobPriorities;


    //Job stuff
    // Keep reference of the job here or in NPCbehaviour ?
    // Or behaviour keep a ref to the model ?

    public NPCModel(NPCData data, Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
        this.hp = data.maxHealth;
        this.typeOfNpc = data.typeOfNPC;
        this.JobPriorities = data.JobPriorities;

    }

    /**
 *  Getter of the tile pos
 */
    public Vector3 GetPos()
    {
        return new Vector3(x, y, z);
    }

    /**
    *  Setter of the tile pos
    */
    public void SetPos(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
}
