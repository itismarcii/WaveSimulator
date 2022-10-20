using System.Diagnostics;
using Extensions;
using Floater;
using ShaderWave;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DepthTest : MonoBehaviour
{
    public Floater.Floater Floater;
    public Transform GridTransform;
    public GridHolder GridHolder;
    private WaveGrid _Grid;
    [Space(20)]
    
    public int FloaterIndex;
    public float WaveHeightY;
    public int GridIndex;

    
    private void Awake()
    {
        MeshTable.SetupTable(1000);
        GridHolder.Setup();
        _Grid = GridHolder._WaveGrid;
        Floater.SetMeshIndex(_Grid, GridIndex);
        MeshTable.SetupTable(100);
    }

    
    private void FixedUpdate()
    {
        WaveHeightY = DepthCalculator.CalculateDepth(ref Floater, _Grid);
        FloaterIndex = Floater.Index;
        GridIndex = Floater.GridIndex;
    }
}
