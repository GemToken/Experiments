using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    private Vector3 scanStartingPos = new Vector3(0.0f, 1.5f, 3.0f);
    private Vector3 hubStartingPos = new Vector3(0.0f, 29.5f, 20.0f);
    public string movementType = "stand";

    public float turnSpeed = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GenericMovement(movementType);
	}

    public void UpdateCharPosition(string newPos)
    {
        switch (newPos){
            case "start":
                this.gameObject.transform.position = scanStartingPos;
                break;
            case "hub":
                this.gameObject.transform.position = hubStartingPos;
                break;
        }
    }

    public void GenericMovement(string type)
    {
        switch(type){
            case "stand":
                break;
            case "wander":
                Wander();
                break;
            case "turn":
                Turn();
                break;
        }
    }

    public void Wander()
    {
        // Figure walks around at a slow pace, random directions, stopping occasionally
    }

    public void Turn()
    {
        transform.Rotate(Vector3.down * Time.deltaTime * turnSpeed);
    }
}

