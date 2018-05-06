using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    // All weather type
    public GameObject[] weather;

	// Use this for initialization
	void Start () {

        //Start a random Weather
        weather[Random.Range(0, weather.Length)].SetActive(true);

	}
}
