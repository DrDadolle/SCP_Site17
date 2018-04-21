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

    // Dictionnary of all the gameobjects furnitures
    public Dictionary<OfficeModel, GameObject> listOfAllOffices;
    public Dictionary<ComputerModel, GameObject> listOfAllComputers;

    public List<GameObject> goArray;

	// OnAwake
	void Awake () {
        listOfAllOffices = new Dictionary<OfficeModel, GameObject>();
        listOfAllComputers = new Dictionary<ComputerModel, GameObject>();
        goArray = new List<GameObject>();
        Instance = this;
    }

    /**
     *  for test purposes only
     */
    private void Update()
    {
        // Just for test purposes
        foreach (var v in listOfAllComputers.Values)
        {
            v.transform.Rotate(Vector3.up * Random.Range(5f, 10f) * Time.deltaTime);
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
     *  Generic method to return list of keys from dic
     */
    public List<T> GetListOfModelFromDictionnary<T>(Dictionary<T,GameObject> dic)
    {
        List<T> ret = new List<T>();
        foreach(var key in dic.Keys)
        {
            ret.Add(key);
        }
        return ret;
    }

    /**
     *  Replace all keys of a dict by the list
     */
     public Dictionary<T, GameObject> ReplaceKeysOfDictByList<T>(Dictionary<T, GameObject> dic, List<T> list)
    {
        // Avoid out of sync issue for dictionnary
        Dictionary<T, GameObject> result = new Dictionary<T, GameObject>();
        T foundElement;

        bool isFound = false;
        foreach(var key in dic.Keys)
        {
            //Reset
            isFound = false;
            foundElement = default(T);

            foreach (var l in list)
            {
                if(l.Equals(key))
                {
                    // Mark as found
                    isFound = true;
                    foundElement = l;
                    //Add the new keys and the old value
                    result.Add(l, dic[key]);

                    // exit the loop
                    break;
                }
            }

            // if found, remove the l element from the list
            if (isFound)
            {
                list.Remove(foundElement);
            }
            //if not found, it means that the save action had an issue
            else
            {
                Debug.LogError("We didn't found the key " + key + " in the list " + list);
            }
        }

        // At the end we replace the dictionnary by the result one
        return result;
    }

    /**
     * Method to rotate all GameObjects
     * TODO : find a way to make it generic
     */
     private void RotateAllGameObjects()
    {
        // Offices
        foreach (var model in listOfAllOffices.Keys)
        {
            listOfAllOffices[model].transform.Rotate(Vector3.up * model.rotationOfTheFurniture);
        }

        //Computers
        foreach (var model in listOfAllComputers.Keys)
        {
            listOfAllComputers[model].transform.Rotate(Vector3.up * model.rotationOfTheFurniture);
        }
    }


    /**
     *  Get the model based on position from all the dictionnary
     */
    public FurnitureModel GetModelFromAllDictionnaries(Vector3Int pos)
    {
        FurnitureModel ret = null;
        //Office
       foreach(var model in listOfAllOffices.Keys)
       {
            if(model.GetTilePos().Equals(pos))
            {
                return model;
            }
       }

        //Computers
        foreach (var model in listOfAllComputers.Keys)
        {
            if (model.GetTilePos().Equals(pos))
            {
                return model;
            }
        }
        return ret;
    }

}
