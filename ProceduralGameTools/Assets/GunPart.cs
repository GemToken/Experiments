using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPart : MonoBehaviour
{
    public Vector3 scaleMin = new Vector3(1, 1, 1);
    public Vector3 scaleMax = new Vector3(1, 1, 1);

    public Vector3 rotateMin = new Vector3(0, 0, 0);
    public Vector3 rotateMax = new Vector3(0, 0, 0);

    public float scaleSnap = 0.0f;
    public float rotateSnap = 0.0f;

    public Color colorMod = new Color(0, 0, 0);

    public Vector3 originScale;
    public Vector3 originAngles;

    // Start is called before the first frame update
    void Start()
    {
        originScale = this.transform.localScale;
        originAngles = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
