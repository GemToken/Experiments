using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGun : MonoBehaviour
{
    public string weaponProfile;

    private Dictionary<string, Transform> gunParts;

    private Color baseColor;
    private Color secondaryColor;
    private Color tertiaryColor;

    // Start is called before the first frame update
    void Start()
    {
        gunParts = new Dictionary<string, Transform>();
        initGunPart(this.transform.Find("Frame"));

        float red = Random.Range(0.0f, 1.0f);
        float green = Random.Range(0.0f, 1.0f);
        float blue = Random.Range(0.0f, 1.0f);
        baseColor = new Color(red, green, blue);

        //Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            float red = Random.Range(0.0f, 1.0f);
            float green = Random.Range(0.0f, 1.0f);
            float blue = Random.Range(0.0f, 1.0f);
            baseColor = new Color(red, green, blue);
            foreach (string key in gunParts.Keys)
            {
                colorPart(gunParts[key], Color.white);
                gunParts[key].localScale = gunParts[key].GetComponent<GunPart>().originScale;
                gunParts[key].localEulerAngles = gunParts[key].GetComponent<GunPart>().originAngles;

                morphRandom(gunParts[key].GetComponent<GunPart>());
                colorRandom(gunParts[key]);
            }
        }
    }


    void Randomize()
    {
        foreach (string key in gunParts.Keys)
        {
            morphRandom(gunParts[key].GetComponent<GunPart>());
            colorRandom(gunParts[key]);
        }
    }


    void initGunPart(Transform part)
    {

        if (part.tag == "GunPart") { gunParts.Add(part.gameObject.name, part); }

        if (part.childCount == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < part.childCount; i++)
            {
                initGunPart(part.GetChild(i));
            }
        }
    }

    Vector3 snapVectorToInt(Vector3 v3, float snap)
    {
        if (snap != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                float newAxis = Mathf.Ceil(v3[i] / snap) * snap;
                v3[i] = newAxis;
            }
        }
        return v3;
    }

    void randomScale(GunPart part)
    {
        Vector3 newScale = new Vector3();
        newScale.x = Random.Range(part.scaleMin.x, part.scaleMax.x);
        newScale.y = Random.Range(part.scaleMin.y, part.scaleMax.y);
        newScale.z = Random.Range(part.scaleMin.z, part.scaleMax.z);
        scalePart(part, newScale);
    }

    void randomRotate(GunPart part)
    {
        Vector3 newRotate = new Vector3();
        newRotate.x = Random.Range(part.rotateMin.x, part.rotateMax.x);
        newRotate.y = Random.Range(part.rotateMin.y, part.rotateMax.y);
        newRotate.z = Random.Range(part.rotateMin.z, part.rotateMax.z);
        rotatePart(part, newRotate);
    }

    void morphRandom(GunPart part)
    {
        // Scale
        randomScale(part);

        // Rotate
        randomRotate(part);
    }

    void colorRandom(Transform part)
    {
        // Colour
        colorPart(part, baseColor - part.GetComponent<GunPart>().colorMod);
    }

    void scalePart(GunPart part, Vector3 scale)
    {
        part.transform.localScale = Vector3.Scale(part.transform.localScale, scale);
    }

    void translatePart(Transform part, float x, float y, float z)
    {
        part.localPosition += new Vector3(x, y, z);
    }

    void rotatePart(GunPart part, Vector3 rotation)
    {
        part.transform.Rotate(rotation, Space.Self);
        part.transform.eulerAngles = snapVectorToInt(part.transform.eulerAngles, part.rotateSnap);
    }

    void colorPart(Transform part, Color color)
    {
        GameObject model = part.Find("_model").gameObject;
        model.GetComponent<MeshRenderer>().material.color = color;
    }
}
