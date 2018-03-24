using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesMethod : MonoBehaviour {

    //Utilities method to change the material of all the children
    public static void ChangeMaterialOfRecChildGameObject(GameObject obj, Material material)
    {
        Renderer[] children;
        children = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = material;
            }
            rend.materials = mats;
        }
    }

    //Return true if at least one material of the game object is from the specified Material
    public static bool IsChildGameObjectOfSpecificMaterial(GameObject obj, Material material)
    {
        Renderer[] children;
        children = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            if (rend.sharedMaterial.Equals(material))
            {
                return true;
            }
        }
        return false;
    }

}
