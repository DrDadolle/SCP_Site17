using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableType/Dictionnary of Position GameObject Variable")]
public class DictionnaryPosGoVariable : ScriptableObject
{
    public Dictionary<Vector3, GameObject> theDict;
    public List<GameObject> l;

    public Material PreviewMaterial;
    public Material FinishMaterial;

    private void OnEnable()
    {
        theDict = new Dictionary<Vector3, GameObject>();
        l = new List<GameObject>();
    }

    public bool AddToDict(Vector3 v, GameObject go)
    {
        if (theDict.ContainsKey(v))
        {
            return false;
        }
        theDict.Add(v, go);
        l.Add(go);
        return true;
    }
}
