using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using ShaderWave;
using UnityEngine;

public class GridHolder : MonoBehaviour
{

    [HideInInspector] public WaveGrid _WaveGrid;

    public void Setup()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        _WaveGrid = new WaveGrid(meshFilters, MeshTable.GetFraction(meshFilters.Length),
            MeshTable.GetFraction(meshFilters[0].mesh.vertexCount));
    }
}
