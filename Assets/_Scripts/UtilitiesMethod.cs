using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesMethod {

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

    /*
	 * Snap the position at the center of the grid
	 * The y axis will be always of 0
	*/
    public static Vector3 snapCenterPosition(Vector3 original)
    {
        Vector3 snapped;
        snapped.x = Mathf.Floor(original.x) + 0.5f;
        snapped.y = Mathf.Floor(0f);
        snapped.z = Mathf.Floor(original.z) + 0.5f;
        return snapped;
    }

    /*
     * Snap the position at the edge of the grid
     * The y axis will be always of 0
    */
    public static Vector3 snapPosition(Vector3 original)
    {
        Vector3 snapped;
        snapped.x = Mathf.Floor(original.x + 0.5f);
        snapped.y = Mathf.Floor(0f);
        snapped.z = Mathf.Floor(original.z + 0.5f);
        return snapped;
    }

}
