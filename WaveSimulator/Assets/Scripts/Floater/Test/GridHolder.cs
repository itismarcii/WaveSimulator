using Extensions;
using ShaderWave;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
    public WaveGrid WaveGrid { get; private set; }

    public WaveGrid Setup()
    {
        var meshFilters = GetComponentsInChildren<MeshFilter>();
        return new WaveGrid(meshFilters);
    }
}
