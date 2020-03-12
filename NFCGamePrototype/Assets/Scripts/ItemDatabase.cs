using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

    public List<GameObject> itemDB = new List<GameObject>();
    public List<GameObject> equipDB = new List<GameObject>();
    public List<GameObject> decorDB = new List<GameObject>();
    public List<GameObject> companionDB = new List<GameObject>();

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject getItem(string code)
    {
        string[] codeArray = code.Split('-');
        GameObject item = null;
        switch (codeArray[0])
        {
            case "I":
                item= itemDB[System.Int32.Parse(codeArray[1])];
                break;
            case "E":
                item = equipDB[System.Int32.Parse(codeArray[1])];
                break;
            case "D":
                item = decorDB[System.Int32.Parse(codeArray[1])];
                break;
            case "C":
                item = companionDB[System.Int32.Parse(codeArray[1])];
                break;
        }
        return item;
    }
}
