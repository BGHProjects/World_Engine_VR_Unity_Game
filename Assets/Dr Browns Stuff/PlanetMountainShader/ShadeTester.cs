using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShadeTester : MonoBehaviour
{
    public GameObject planet;
    public Color color;
    public float red = 0.0f;
    public float blue = 0.5f;

    public float displaceFactor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShaderControls();

        color = new Color(red, 0.0f, blue, 1.0f);

        planet.GetComponent<Renderer>().sharedMaterial.SetColor("_SurfaceColor", color);

        planet.GetComponent<Renderer>().sharedMaterial.SetFloat("_DisplaceFactor", displaceFactor);

    }

    void ShaderControls()
    {
        // Changes the surface colour

        if(Input.GetKey("q"))
        {
            //color = new Color(0.5f, 0.0f, 0.0f, 1.0f);
            if (red > 0.0f)
            {
                red -= 0.01f;
            }

                if (blue < 0.5f)
                {
                    blue += 0.01f;
                }

        }

        if (Input.GetKey("e"))
        {
            //color = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            if(blue > 0.0f)
            {
                blue -= 0.01f;
            }

                if(red < 0.5f)
                {
                    red += 0.01f;
                }

        }

        //Changes the displacement height

        if (Input.GetKey("w"))
        {
            if (displaceFactor < 1.0f)
            {
                displaceFactor += 0.05f;
            }
        }


        if (Input.GetKey("s"))
        {
            if (displaceFactor > 0.0f)
            {
                displaceFactor -= 0.05f;
            }
        }


    }
}
