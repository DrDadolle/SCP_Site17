using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Building Method for Walls
 */
public class CreateWall : MonoBehaviour, IBuildingMethod {

	private Vector3 it;
	private Vector3 lastIteration;

	/*keep in memory which axis we were building on;
	 * 0 = none
	 * 1 = X
	 * 2 = Z
	 */
	public int buildingAxis = 0;

    //Tag of the Pole
    private static string POLE_TAG = "Pole";

	//Show the ending point of the wall
	private GameObject endPole;

	// Array of provisiorary wall's GO
	private List<GameObject> tempGOList;

	//Show the starting point of the wall
	public GameObject startPole {
        get; private set;
    }

	//Store the starting position of the drag and drop
	public Vector3 startPosition;

	//Public game objects models
	public GameObject polePrefab;
	public GameObject wallPrefab;

	// Materials
	private Object[] tmp_materials;
	protected static string PATH_TO_WALL_MATERIALS = "Materials_Wall";
	public static Material default_material;
	public static Material ghostly_blue;
	public static Material ghostly_red;

	//Value of the layer
	public static int VALUE_OF_WALLS_LAYER = 10;
	public static int VALUE_OF_GHOST_WALLS_LAYER = 9;


	//The instance for access purposes
	public static CreateWall Instance;




	//On Awake
	void Awake(){
		Instance = this;
        //Create a temp WallGameObject list
        tempGOList = new List<GameObject> ();

        //Loading the 3 differents materials for walls
        LoadingWallMaterials();
	}

    // ============================== Implement IBuildingMethod ==============================
    // These methods are called by the MouseController

    // OnLeftButtonPress
    // Used for placing object or initiating drag and dropping
    public virtual void OnLeftButtonPress(MouseController pointer)
    {
        //When clicking for the first time, Sets the starting pole and all the variables
        StartWall(pointer);

    }

    // OnLeftButtonReleaseDuringDragAndDrop
    // Used to finish a drag and drop of the left mouse button
    public virtual void OnLeftButtonReleaseDuringDragAndDrop(MouseController pointer)
    {
        //Create the effective walls
        SetWall();
    }

    // DuringDragAnDrop
    // Method called during each frame of a drag and drop
    public virtual void DuringDragAndDrop(MouseController pointer)
    {
        //Update the walls according to the mouse position
		UpdateWall(GetCurrentMousePosition(pointer));
    }

    // OnRightButtonPress
    // Used for canceling stuff
    public virtual void OnRightButtonPressDuringDragAndDrop(MouseController pointer)
    {
        // Remove the starting Pole
        Destroy(startPole);

        // Remove all walls
        ClearAndDestroyAllGO();
    }

    // ============================== End of IBuildingMethods ==============================

    //Initiate the wall segment creation
	public void StartWall(MouseController pointer){

		// Set the starting Position
		Vector3 startPos = pointer.getWorldPoint();
		startPosition = pointer.snapPosition (startPos);
		lastIteration = startPosition;

		// Show the starting position with a gameObject
		startPole = CreatePoles(polePrefab, startPosition);
	}


	//Set definitive walls
    //Remove unneeded poles
    //Add poles to fix wall apparence
	public void SetWall(){

		// Show the finishing position with a gameObject only if there is no such pole already existing
		endPole = CreatePoles(polePrefab,lastIteration);


        //Delete End Pole if near a red wall ONLY if endPole exists
         DestroyPoleNearOnlyRedWalls(endPole);

        //Delete the starting pole if it is near a red wall ONLY
        DestroyPoleNearOnlyRedWalls(startPole);

        //If just clicking on the same place, do nothing
        // Should not be any NPE here
        if (lastIteration.Equals(startPosition))
        {
            Destroy(startPole);
        }

        // Adding poles to walls in case of an obstacle
        //Might get reworked to include endPole case
        AutoAddingPolesToWalls();

        // Change the material of walls
        foreach (var w in tempGOList) {
			DestroyCollidingWalls (w);
			ChangePropertiesOfWall (w, default_material, false);
		}

        //Clear the previous arraylist
        tempGOList.Clear();
    }


    //Show the future position of the wall
	public void UpdateWall(Vector3 currentPosition)
    {     
        if (!currentPosition.Equals(lastIteration))
        {
            RecursiveWallBuilder(startPosition, currentPosition);
        }
    }

	//Get the current position of the Mouse
	public Vector3 GetCurrentMousePosition(MouseController pointer){
		Vector3 currentPosition = pointer.getWorldPoint();
		currentPosition = pointer.snapPosition(currentPosition);
		currentPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
		return currentPosition;
	}

    //Check the nearby walls of the pole and destroy it if it is near a red pole
    void DestroyPoleNearOnlyRedWalls(GameObject pole)
    {
        //Delete End Pole if near a red wall ONLY if endPole exists
        if (pole != null)
        {
            List<GameObject> walls_near_pole = GetWallsNearPoles(pole);
            if (walls_near_pole.Count > 1)
            {
                //Impossible case
                Debug.LogError(pole.name + "can't be near 2 Ghostly Walls. Destroying it");
                Destroy(pole);
            }
            else if (walls_near_pole[0].GetComponent<Renderer>().sharedMaterial.Equals(ghostly_red))
            {
                //EndPole near a ghostly red wall : destroy endpole
                Destroy(pole);
            }
            //else keep it
        }
    }


    // Adding poles to walls in case of an obstacle
    //Might get reworked to include endPole case
    void AutoAddingPolesToWalls()
    {
        //Add poles to fix walls apparence
        for (int i = 0; i < tempGOList.Count - 1; i++)
        {
            GameObject wall_i = tempGOList[i];
            GameObject wall_i_plus_1 = tempGOList[i + 1];

            //common case
            //Walls should be red OR blue only
            //When interfacing red and blue, put a pole between them
            if (!wall_i.GetComponent<Renderer>().sharedMaterial.Equals(wall_i_plus_1.GetComponent<Renderer>().sharedMaterial))
            {
                Vector3 tmpv3 = wall_i.transform.position;

                //x building case
                if (buildingAxis == 1)
                {
                    //Positive building
                    if (wall_i.transform.position.x < wall_i_plus_1.transform.position.x)
                        tmpv3.x += 0.5f;
                    //Negative building
                    else
                        tmpv3.x -= 0.5f;
                    //z building case
                }
                else
                {
                    if (wall_i.transform.position.z < wall_i_plus_1.transform.position.z)
                        tmpv3.z += 0.5f;
                    //Negative building
                    else
                        tmpv3.z -= 0.5f;
                }
                CreatePoles(polePrefab, tmpv3);
            }

            //Limit case
            //Not needed ?
        }
    }


	private void DestroyCollidingWalls(GameObject wall){
		//The wall is still red because of a collision
		if (wall.GetComponent<Renderer> ().sharedMaterial.Equals(ghostly_red)) {
			Destroy (wall);
		}
	}


	/**
	 * Create recursively walls between a start position and an end position
	 * StartPos and finishPos are both snapped already
	 * 
	 */
	void RecursiveWallBuilder(Vector3 startPos, Vector3 finishPos)
	{
		int _max_x = Mathf.RoundToInt (finishPos.x - startPos.x);
		int max_x = Mathf.Abs (_max_x); 
		int _max_z = Mathf.RoundToInt (finishPos.z - startPos.z);
		int max_z = Mathf.Abs (_max_z); 

			//if it is an x-axis building
			//Default case
		if (max_x >= max_z) {
			if (max_x > tempGOList.Count) {
				lastIteration = startPos;
				for (int i = 0; i < max_x; i++) {
					buildingAxis = 1;
					CreateWallSegment (i + 1, _max_x > 0);
				}
			} else {
				DestroyObsoleteGO (_max_x);
		
			}
		}
		//else if its an z-axis building
		else {
			if (max_z > tempGOList.Count) {
				lastIteration = startPos;
				for (int i = 0; i < max_z; i++) {
					buildingAxis = 2;
					CreateWallSegment (i + 1, _max_z > 0);
				}
			} else {
				DestroyObsoleteGO (_max_z);
			}
		}
	}

	//==========================================
	//Create effectively a Segment of the Wall
	void CreateWallSegment(int iteration, bool isPositive){

		if (!isPositive)
			iteration = -iteration;

		//Check that all the object are build on the same axis (x or z one)
		CheckAllObjectOnXorZAxis (iteration);

		//Check that all the object are build on the same side (negative or positive one)
		CheckAllObjectPositiveOrNegativeAxis ();

		// checks if it is a new item
		if (Mathf.Abs(iteration) > tempGOList.Count) {

			//Get the middle between the 2 last points
			Vector3 middle = Vector3.Lerp (lastIteration, it, 0.5f);

			//Create the wall
			GameObject newWall = Instantiate (wallPrefab, middle, Quaternion.identity);

            //Add it to the arraylist
            tempGOList.Add (newWall);

			// Rotate the model so the forward axis points in the right direction
			Vector3 YRotation = new Vector3 (0, 90, 0);
			newWall.transform.LookAt (startPosition);
			newWall.transform.Rotate (YRotation);
		}
		//Update the last iteration
		lastIteration = it;
	}


	//Loading of Wall Materials Resources
	private void LoadingWallMaterials()
	{
		tmp_materials = Resources.LoadAll (PATH_TO_WALL_MATERIALS, typeof(Material));
		foreach (var t in tmp_materials)
		{
			if (t.name == "Walls"){
				default_material = (Material) t;
			} else if (t.name == "Wall_Ghost_Blue") {
				ghostly_blue = (Material) t;
			} else if (t.name == "Wall_Ghost_Red"){
				ghostly_red = (Material) t;
			}
		}

		if (default_material == null || ghostly_blue == null || ghostly_red == null)
			Debug.LogError ("Loading of Wall Materials failed");
	}
		
	//Create Poles at atPos coordinates
	GameObject CreatePoles(GameObject poleprefab, Vector3 atPos){
		
		GameObject polCreated = null;
		bool isDup = false;
		GameObject[] arrayOfPoles = GameObject.FindGameObjectsWithTag (POLE_TAG);
        
        //If there is already a Pole at this position, don't create a new pole
		foreach (var p in arrayOfPoles) {
			if (p.transform.position.Equals (atPos)) {
				isDup = true;
			}
		}

		if (!isDup) {
			polCreated = Instantiate (poleprefab, atPos, Quaternion.identity);
			polCreated.transform.position = new Vector3 (atPos.x, atPos.y, atPos.z);
		}
		return polCreated;
	}

    //Check if there is nearby walls around Pole
    List<GameObject> GetWallsNearPoles(GameObject pole)
    {
        List<GameObject> nearbyWalls = new List<GameObject>();
        //Get the list of all nearby walls
        foreach (var w in tempGOList)
        {
            if((Mathf.Abs(w.transform.position.x - pole.transform.position.x) <= 0.5  && buildingAxis == 1)
                || (Mathf.Abs(w.transform.position.z - pole.transform.position.z) <= 0.5) && buildingAxis == 2)
            {
                nearbyWalls.Add(w);
            }
        }
        return nearbyWalls;
    }

    // Change the properties of the materials and physics for a specific piece of wall
    public static void ChangePropertiesOfWall(GameObject wall, Material mat, bool setIsTrigger)
    {
        // Set the materials
        wall.GetComponent<Renderer>().material = mat;

        //Set the trigger
        Collider tmp_collider = wall.GetComponent<Collider>();
        tmp_collider.isTrigger = setIsTrigger;

        //If we are removing the trigger, it means we are finally building a wall for real !
        if (setIsTrigger == false)
        {
            //Changing its layer from GhostWalls (10) to Walls (9)
            wall.layer = VALUE_OF_WALLS_LAYER;
            Rigidbody tmp_rigid = wall.AddComponent<Rigidbody>();
            tmp_rigid.isKinematic = true;
        }

    }

    //Check that all the object are build on the same axis (x or z one)
    void CheckAllObjectOnXorZAxis(int iteration){
		
		//Create the it vector indicating the new point of the wall
		if (buildingAxis == 1) {
			it = new Vector3 (startPosition.x + iteration, startPosition.y, startPosition.z);
		} else {
			it = new Vector3 (startPosition.x, startPosition.y, startPosition.z + iteration);
		}
			
		// Check if we are still building on the right axis
		if (tempGOList.Count > 0) {
			if (buildingAxis == 1 && tempGOList[0].transform.position.z != startPosition.z) {
				buildingAxis = 2;
				ClearAndDestroyAllGO ();
			} else if (buildingAxis == 2 && tempGOList[0].transform.position.x != startPosition.x) {
				buildingAxis = 1;
				ClearAndDestroyAllGO ();
			}
		}
	}

	//Check that all the object are build on the same side (negative or positive one)
	void CheckAllObjectPositiveOrNegativeAxis(){
		if (tempGOList.Count > 0) {
			if (buildingAxis == 1) {
                if (it.x > startPosition.x && tempGOList[0].transform.position.x < startPosition.x)
                    ClearAndDestroyAllGO();
                else if (it.x < startPosition.x && tempGOList[0].transform.position.x > startPosition.x)
                    ClearAndDestroyAllGO();
			} else {
				if (it.z > startPosition.z && tempGOList[0].transform.position.z < startPosition.z)
					ClearAndDestroyAllGO ();
				else if (it.z < startPosition.z && tempGOList[0].transform.position.z > startPosition.z)
					ClearAndDestroyAllGO ();
			}
		}
	}
		
	//Utility Method to empty the gameObject Array
	public void ClearAndDestroyAllGO()
	{
        tempGOList.ForEach(delegate(GameObject obj) {
			Destroy(obj);
		});
        tempGOList.Clear ();
	}


	//destroy
	void DestroyObsoleteGO(int pseudoMax){
		int pseudoMaxAbs = Mathf.Abs (pseudoMax);

		for (int i = pseudoMaxAbs  ; i < tempGOList.Count; i++) {
			if (pseudoMax > 0) {
				if (buildingAxis == 1) {
					lastIteration.x -= 1;
				} else {
					lastIteration.z -= 1;
				}
			} else if (pseudoMax < 0) {
				if (buildingAxis == 1) {
					lastIteration.x += 1;
				} else {
					lastIteration.z += 1;
				}
			} else {
				if (buildingAxis == 1) {
					lastIteration.x = startPosition.x;
				} else {
					lastIteration.z = startPosition.z;
				}
			}

			/*if (endPole != null) 
				endPole.transform.position = lastIteration;
			*/

			Destroy (tempGOList[i]);
            tempGOList.RemoveAt (i);
		}
	}

}
