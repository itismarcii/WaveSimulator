using Extensions;
using Floater;
using UnityEngine;

public class DepthTest : MonoBehaviour
{
    public Floater.Floater Floater;
    public MeshFilter MeshFilter;
    private Mesh _Mesh;
    
    [Space(20)]
    
    public int FloaterIndex;
    public float WaveHeightY;
    
    private void Awake()
    {
        _Mesh = MeshFilter.mesh;
        Floater.SetMesh(MeshFilter);
        MeshTable.SetupTable(100);
    }

    private void FixedUpdate()
    {
        WaveHeightY = DepthCalculator.CalculateDepth(MeshFilter, ref Floater);
        FloaterIndex = Floater.Index;
    }
}
