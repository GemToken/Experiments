using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float MoveSpeed = 1;
    public float lookSpeed = 20;

    private Rigidbody rig;
    private bool wallHit;

	// Use this for initialization
	void Start () {
        rig = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rig.MoveRotation(rig.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * lookSpeed, 0)));
        if (!wallHit)
        {
            rig.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Wall"))
        {
            wallHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Wall"))
        {
            wallHit = false;
        }
    }
}
