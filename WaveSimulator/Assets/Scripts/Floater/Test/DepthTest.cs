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
        var gridMatrix = MeshTable.GetFraction(_Grid.MeshGroup.Length);
        var resolution = MeshTable.GetFraction(_Grid.MeshGroup[0].mesh.vertexCount);
        var minDistance = float.MaxValue;
        var position = GridTransform.position;
        for (var i = 0; i < gridMatrix; i++)
        {
            for (var j = 0; j < gridMatrix; j++)
            {
                var distance = Vector3.Distance(
                    new Vector3(position.x + i * resolution, 0, position.z + j * resolution), 
                    transform.position);
                if (distance >= minDistance) continue;
                minDistance = distance;
                GridIndex = i + j;
            }
        }
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
