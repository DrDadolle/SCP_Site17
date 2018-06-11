using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Hide_Menu : MonoBehaviour {

	public void Show_Hide()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
