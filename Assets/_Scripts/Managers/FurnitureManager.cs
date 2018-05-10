using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 *  Will manage state of all furnitures
 */
public class FurnitureManager : MonoBehaviour {

    // For access purpose
    public static FurnitureManager Instance;

    //Map of the world
    public Tilemap world;

    // Struct of the OfficeObject
    public struct OfficeObject
    {
        public OfficeModel model;
        public GameObject go;

        public OfficeObject(OfficeModel m, GameObject g)
        {
            model = m;
            go = g;
        }
    }

    // Struct of the ComputerObject
    public struct ComputerObject
    {
        public ComputerModel model;
        public GameObject go;

        public ComputerObject(ComputerModel m, GameObject g)
        {
            model = m;
            go = g;
        }
    }

    // Dictionnary of all the gameobjects furnitures
    public Dictionary<Vector3Int, OfficeObject> listOfAllOffices;
    public Dictionary<Vector3Int, ComputerObject> listOfAllComputers;

    // OnAwake
    void Awake () {
        listOfAllOffices = new Dictionary<Vector3Int, OfficeObject>();
        listOfAllComputers = new Dictionary<Vector3Int, ComputerObject>();
        Instance = this;
    }

    /**
     *  for test purposes only
     */
    private void Update()
    {
        foreach (var v in listOfAllComputers.Values)
        {
            v.go.transform.Rotate(Vector3.up * Random.Range(5f, 10f) * Time.deltaTime);
        }
        foreach (var v in listOfAllOffices.Values)
        {
            v.go.transform.Rotate(Vector3.up * Random.Range(5f, 10f) * Time.deltaTime);
            //Debug.Log(v.rotationOfTheFurniture);
        }
    }

    /**
     *  Clear all furniture data
     */
     public void ClearAll()
    {
        listOfAllOffices.Clear();
        listOfAllComputers.Clear();
    }

    /**
     *  Called when GameObjects got loaded
     *  TODO : make it generic
     */
    public void OnLoading()
    {
        RotateAllGameObjects();
    }

    /**
     * Method to rotate all GameObjects
     * TODO : find a way to make it generic
     */
     private void RotateAllGameObjects()
    {
        // Offices
        foreach (var v in listOfAllOffices.Values)
        {
            v.go.transform.Rotate(Vector3.up * v.model.rotationOfTheFurniture);
        }

        //Computers
        foreach (var v in listOfAllComputers.Values)
        {
            v.go.transform.Rotate(Vector3.up * v.model.rotationOfTheFurniture);
        }
    }


    /**
     *  Get the model based on position from all the dictionnary
     */
    public FurnitureModel GetModelFromAllDictionnaries(Vector3Int pos)
    {
        FurnitureModel ret = null;

        //Office
        if (listOfAllOffices.ContainsKey(pos))
        {
            return listOfAllOffices[pos].model;
        }

        //Computers
        if (listOfAllComputers.ContainsKey(pos))
        {
            return listOfAllComputers[pos].model;
        }
        return ret;
    }

    /**
     *  Remove model from the dict.
     *  False = not removed
     */
     public bool RemoveModelFromDictionnaries(Vector3Int pos, bool preview, bool pending)
    {
        //Office
        if(listOfAllOffices.ContainsKey(pos))
        {
            OfficeModel _office = listOfAllOffices[pos].model;
                if (_office.isPending != pending || _office.isPreview != preview)
                    return false;
                else
                    return (listOfAllOffices.Remove(pos));
        }

        //Computers
        if (listOfAllComputers.ContainsKey(pos))
        {
            ComputerModel _computer = listOfAllComputers[pos].model;
            if (_computer.isPending != pending || _computer.isPreview != preview)
                return false;
            else
                return (listOfAllComputers.Remove(pos));
        }

        // Not found
        return false;
    }

    /**
    *  Get the model based on position from all the dictionnary
    */
    public bool AddGameObjectToModelFromAllDictionnaries(Vector3Int pos, string TypeOfFurniture, GameObject go)
    {
        switch (TypeOfFurniture)
        {
            case "Office":
                if (listOfAllOffices.ContainsKey(pos))
                {
                    listOfAllOffices[pos] = new OfficeObject(listOfAllOffices[pos].model, go);
                    return true;
                }
                break;
            case "Computer":
                if (listOfAllComputers.ContainsKey(pos))
                {
                    listOfAllComputers[pos] = new ComputerObject(listOfAllComputers[pos].model, go);
                    return true;
                }
                break;
        }
        return false;
    }

}
