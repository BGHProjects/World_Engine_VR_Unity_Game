using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighResShapeGenerator
{

    HighResShapeSettings settings;

    public HighResShapeGenerator(HighResShapeSettings settings)
    {
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * settings.planetRadius;
    }
}

