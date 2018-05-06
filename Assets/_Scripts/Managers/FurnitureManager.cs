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

	// OnAwake
	void Awake () {
        listOfAllOffices = new Dictionary<OfficeModel, GameObject>();
        listOfAllComputers = new Dictionary<ComputerModel, GameObject>();
        Instance = this;
    }

    /**
     *  for test purposes only
     */
    private void Update()
    {
        foreach (var v in listOfAllComputers.Values)
        {
            v.transform.Rotate(Vector3.up * Random.Range(5f, 10f) * Time.deltaTime);
        }
        foreach (var v in listOfAllOffices.Values)
        {
            //v.transform.Rotate(Vector3.up * Random.Range(5f, 10f) * Time.deltaTime);
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
        foreach (var model in listOfAllOffices.Keys)
        {
            //Debug.Log(listOfAllOffices[model].name);
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
