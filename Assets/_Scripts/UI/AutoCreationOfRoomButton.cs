using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCreationOfRoomButton : MonoBehaviour {

    // The prefab used to create the first Room Button
    public GameObject RoomButtonPrefab;

    // All the FloorTile to generate
    public List<FloorTile> listOfFloorTile;


	// Use this for initialization
	void Start () {
        foreach(var v in listOfFloorTile)
        {
            GameObject childObject = GameObject.Instantiate(RoomButtonPrefab);
            childObject.transform.parent = gameObject.transform;
            childObject.name = v.name + "_Button";

        }
		
	}



}
