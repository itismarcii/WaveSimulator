using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Extensions;
using ShaderWave;
using UnityEngine;

public class MeshChunkGenerator : MonoBehaviour
{
    #region Structs

        [StructLayout(LayoutKind.Sequential)]
        private struct GlobalVars
        {
            public int resolution;
            public Vector2 chunkId;
            public Vector4 wave;
    
            public void UpdateShift(Vector2 vector2) => chunkId = vector2;
            public void UpdateWve(Vector4 vector4) => wave = vector4;
        }
        
    #endregion

    #region Variables

        [SerializeField] private MeshFilter _MeshFilter;
        [SerializeField, Range(0.01f, 0.1f)] private float _WaveSpeed = 1;
        [SerializeField] private Wave _Wave;
        private int _Resolution;
        private static GlobalVars _GlobalVars;
        private static float _GlobalTime;
    
        [SerializeField] private ComputeShader _Shader;

        [NonSerialized] private ComputeBuffer
            _VerticesOutputBuffer,
            _UVOutputBuffer,
            _TriangleOutputBuffer;

        private static readonly int
            VerticesOutputPropertyId = Shader.PropertyToID("verticesOutput"),
            UVOutputPropertyId = Shader.PropertyToID("uvOutput"),
            TriangleOutputPropertyId = Shader.PropertyToID("triangleOutput"),
            TimePropertyId = Shader.PropertyToID("time");
    
    #endregion

    private void Start()
    { 
        Setup();
        SetupMesh();
        
        UpdateGerstnerWave();
    }
    
    private void FixedUpdate()
    {
        UpdateGerstnerWave();
    }

    private void OnDisable()
    {
        _VerticesOutputBuffer.Dispose();
    }

    private void Setup()
    {
        MeshTable.SetupTable(1000);
        _Resolution = MeshTable.GetFraction(_MeshFilter.mesh.vertexCount);
        var chunkRowCol = _Resolution / 32;

        if (chunkRowCol < 1) chunkRowCol = 1;
        
        _GlobalVars = new GlobalVars()
        {
            resolution = _Resolution, 
            chunkId = new Vector2(chunkRowCol, chunkRowCol),
            wave = new Vector4(_Wave.X, _Wave.Z, _Wave.Amplitude, _Wave.Wavelength)
        };
        
        _VerticesOutputBuffer = new ComputeBuffer(_MeshFilter.mesh.vertexCount, sizeof(float) * 3);
        
        _Shader.SetFloat(Shader.PropertyToID("scaling"), 10 / (float)_Resolution);
        _Shader.SetInt(Shader.PropertyToID("resolution"), _GlobalVars.resolution);
        _Shader.SetVector(Shader.PropertyToID("wave"), _GlobalVars.wave);
        _Shader.SetVector(Shader.PropertyToID("chunkId"), _GlobalVars.chunkId);

        _GlobalTime = 0;
    }


    private void SetupMesh()
    {
        var mesh = _MeshFilter.mesh;

        using (_UVOutputBuffer = new ComputeBuffer(mesh.uv.Length, sizeof(float) * 2))
        {
            mesh.uv = GetBufferData(1, _UVOutputBuffer, UVOutputPropertyId, mesh.uv);
        }
        
        using (_TriangleOutputBuffer = new ComputeBuffer(mesh.triangles.Length, sizeof(int)))
        {
            mesh.triangles = GetBufferData(2, _TriangleOutputBuffer, TriangleOutputPropertyId, mesh.triangles);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T[] GetBufferData<T>(int kernel, ComputeBuffer buffer, int propertyId, T[] data)
    {
        if (buffer == null) Debug.Log("No buffer");
        
        using (buffer = new ComputeBuffer(data.Length, Marshal.SizeOf<T>()))
        {
            _Shader.SetBuffer(kernel, propertyId, buffer);
            _Shader.Dispatch(kernel, 32 , 1, 32);
            var bufferData = new T[data.Length];
            buffer.GetData(bufferData);
            return bufferData;
        }
    }

    private void UpdateGerstnerWave()
    {
        var mesh = _MeshFilter.mesh;
        _Shader.SetFloat(TimePropertyId, _GlobalTime);
        _Shader.SetBuffer(0, VerticesOutputPropertyId, _VerticesOutputBuffer);
        _Shader.Dispatch(0, 32, 1, 32);

        var verticesData = new Vector3[mesh.vertexCount];
        _VerticesOutputBuffer.GetData(verticesData);
        mesh.vertices = verticesData;

        _MeshFilter.mesh = mesh;

        _GlobalTime += Time.fixedTime * _WaveSpeed;
    }
}
