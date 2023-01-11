using Extensions;
using ShaderWave;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
    [HideInInspector] public WaveGrid _WaveGrid;

    public void Setup()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        _WaveGrid = new WaveGrid(meshFilters);
    }
}
