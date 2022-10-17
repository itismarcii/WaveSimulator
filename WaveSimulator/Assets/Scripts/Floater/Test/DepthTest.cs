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
        Floater.SetMesh(_Mesh);
        MeshTable.SetupTable(1000);
    }

    private void FixedUpdate()
    {
        WaveHeightY = DepthCalculator.CalculateDepth(_Mesh, ref Floater);
        FloaterIndex = Floater.Index;
    }
}
