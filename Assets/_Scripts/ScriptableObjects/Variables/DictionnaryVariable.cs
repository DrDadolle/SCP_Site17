using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableType/Dictionnary Variable")]
public class DictionnaryVariable : ScriptableObject
{
    public Dictionary<Vector3, string> theDict = new Dictionary<Vector3, string>(); 

    public bool AddToDict(Vector3 v, string s)
    {
        if(theDict.ContainsKey(v))
        {
            return false;
        }
        theDict.Add(v, s);
        return true;
    }

    public bool RemoveFromDict(Vector3 v)
    {
        if (theDict.ContainsKey(v))
        {
            theDict.Remove(v);
            return true;
        }
        return false;
    }
}
