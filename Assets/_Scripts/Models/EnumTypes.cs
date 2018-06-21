using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumTypes {

    public enum JobMacroType
    {
        Building,
        Research
    };

    //Classification
    public enum SCPClassification
    {
        Safe,
        Euclid,
        Keter
    };

    public enum TypeOfRoom
    {
        Scientific,
        Military,
        Containement,
        Corridor
    }

    public enum TypeOfNPC
    {
        Researcher,
        Containement_Engineer,
        D_Class,
        SCP
    }
}
