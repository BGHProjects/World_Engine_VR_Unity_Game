using UnityEngine;
using SimplexNoise;
using System.Collections;

public class MyNoise : MonoBehaviour 
{
	private Texture3D tex;
	private int size = 64;
	
	void Start()
	{
		tex = new Texture3D (size, size, size, TextureFormat.ARGB32, true);	
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
		GetComponent<Renderer>().material.SetTexture ("_MainTex", tex);
	}
	
	void Update()
	{
	}
}