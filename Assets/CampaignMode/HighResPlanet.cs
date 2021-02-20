using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighResPlanet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public HighResShapeSettings shapeSettings;
    public HighResColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colourSettingsFoldout;

    HighResShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    HighResTerrainFace[] terrainFaces;


    void Initialize()
    {
        shapeGenerator = new HighResShapeGenerator(shapeSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new HighResTerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Custom/TestShader"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new HighResTerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    void GenerateMesh()
    {
        foreach (HighResTerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColours()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }
}
