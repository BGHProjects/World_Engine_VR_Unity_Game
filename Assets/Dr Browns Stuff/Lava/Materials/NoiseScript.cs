using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
using System.Collections;


public class NoiseScript : MonoBehaviour
{
	private Texture3D tex;
	private int size = 64;
	private GameObject origCam;
	private Matrix4x4 viewI;

	void Start()
	{
		//renderer.material.SetFloat("_NoiseTexRes", size);
		/*
		origCam = GameObject.FindWithTag("MainCamera");
		viewI = origCam.GetComponent<Camera>().cameraToWorldMatrix;
		GetComponent<Renderer>().material.SetMatrix("_ViewI", viewI);
		*/

		tex = new Texture3D(size, size, size, TextureFormat.ARGB32, true);
		var cols = new Color[size * size * size];

		int idx = 0;
		Color c = Color.white;

		for (int z = 0; z < size; ++z)
		{
			for (int y = 0; y < size; ++y)
			{
				for (int x = 0; x < size; ++x, ++idx)
				{
					c.r = Noise.Generate(x, y, z);
					c.g = c.r;
					c.b = c.r;
					cols[idx] = c;
				}
			}
		}

		tex.SetPixels(cols);
		tex.Apply();
		GetComponent<Renderer>().material.SetTexture("VolumeTexture", tex);
	}

	void Update()
	{
		/*
		viewI = origCam.GetComponent<Camera>().cameraToWorldMatrix;
		GetComponent<Renderer>().material.SetMatrix("_ViewI", viewI);
		*/
		//transform.Rotate(Vector3.up * Time.deltaTime * 10.0f);
	}
}
