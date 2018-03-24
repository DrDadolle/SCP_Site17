using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class containing the loaded resources (e.g. Materials)
 */
public class ResourcesLoading : MonoBehaviour {

    // Materials
    private Object[] tmp_materials;
    protected static string PATH_TO_GHOST_MATERIALS = "Materials_Ghost";
    public static Material default_material_wall;
    public static Material ghostly_blue;
    public static Material ghostly_red;


    // Awake
    void Awake () {
        //Loading Materials for Ghostly effects 
        LoadingGhostlyMaterials();

    }


    // Loading Ghostly Materials
    private void LoadingGhostlyMaterials()
    {
        tmp_materials = Resources.LoadAll(PATH_TO_GHOST_MATERIALS, typeof(Material));
        foreach (var t in tmp_materials)
        {
            if (t.name == "Walls")
            {
                default_material_wall = (Material)t;
            }
            else if (t.name == "Ghost_Blue")
            {
                ghostly_blue = (Material)t;
            }
            else if (t.name == "Ghost_Red")
            {
                ghostly_red = (Material)t;
            }
        }

        if (default_material_wall == null || ghostly_blue == null || ghostly_red == null)
            Debug.LogError("Loading of Ghostly Materials failed");
    }
}
