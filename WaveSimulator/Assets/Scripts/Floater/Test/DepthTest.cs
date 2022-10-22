using System.Diagnostics;
using Extensions;
using Floater;
using ShaderWave;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DepthTest : MonoBehaviour
{
    public Floater.Floater Floater;
    public WaveManager Manager;
    private WaveGrid _Grid;
    [Space(20)]
    
    public int FloaterIndex;
    public float WaveHeightY;
    public int GridIndex;

    
    private void Start()
    {
        Floater.Transform = transform;
        Floater.Rigidbody = GetComponent<Rigidbody>();
        MeshTable.SetupTable(1000);
        _Grid = Manager.GetWaveGrid(0);
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
