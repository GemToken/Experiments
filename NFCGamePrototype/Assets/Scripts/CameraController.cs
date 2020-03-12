using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera mainCamera;
    public GameObject playerCharacter;

    // Use this position/rotation to set the camera back to intial location (used for Scan, Shop, Customization)
    private Vector3 startPos = new Vector3(0.0f, 2.5f, -4.0f);
    private Quaternion startRot = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    // Use this position/rotation for setting the camera to the hub room
    private Vector3 hubPos = new Vector3(0.0f, 50.0f, -7.0f);
    private Quaternion hubRot = Quaternion.Euler(35.0f, 0.0f, 0.0f);

    // Use this position/roation to set up camera for Raise Mode
    private Vector3 raisePos = new Vector3(0.0f, 32.0f, 12.0f);
    private Quaternion raiseRot = Quaternion.Euler(8.0f, 0.0f, 0.0f);
    private bool raiseFlag = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateCameraPosition(string newPos)
    {
        switch (newPos)
        {
            case "start":
                mainCamera.transform.position = startPos;
                mainCamera.transform.rotation = startRot;
                raiseFlag = false;
                break;
            case "hub":
                mainCamera.transform.position = hubPos;
                mainCamera.transform.rotation = hubRot;
                raiseFlag = false;
                break;
            case "raise":
                mainCamera.transform.position = raisePos;
                mainCamera.transform.rotation = raiseRot;
                raiseFlag = true;
                break;
        }
    }



}
