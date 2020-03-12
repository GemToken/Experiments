using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProfiles : MonoBehaviour
{
    public WeaponProfile[] weaponProfiles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class WeaponProfile
    {
        // This class can be used to hold information on different weapon archetypes.
        // Useful for coordiated randomness, i.e. if you want "Pistols" versus "Shotguns", etc.
        public string name;

        public Vector3 FrameScale = new Vector3(1, 1, 1);
        public Vector3 FrameRotate = new Vector3(0, 0, 0);

        public Vector3 BarrelScale = new Vector3(1, 1, 1);
        public Vector3 BarrelRotate = new Vector3(0, 0, 0);

        public Vector3 GripScale = new Vector3(1, 1, 1);
        public Vector3 GripRotate = new Vector3(0, 0, 0);
    }
}
