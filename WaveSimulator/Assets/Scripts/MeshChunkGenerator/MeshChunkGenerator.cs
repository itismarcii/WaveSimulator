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
        [SerializeField, Range(0.01f, 5f)] private float _WaveSpeed = 1;
        [SerializeField] private int _WaveAmount = 1;
        [SerializeField, Range(.0001f, 1f)] private float _WaveAmplitudeScale = 1;
        [SerializeField, Range(.0001f, 1f)] private float _WaveTimeShiftFactor = 1;
        [SerializeField] private Wave _Wave0, _Wave1, _Wave2;
        private Vector4[] _Waves;
        private int _Resolution;
        private static GlobalVars _GlobalVars;
        private static float _GlobalTime = 0;
    
        [SerializeField] private ComputeShader _Shader;

        [NonSerialized] private ComputeBuffer
            _VerticesOutputBuffer,
            _UVOutputBuffer,
            _TriangleOutputBuffer;

        private static readonly int
            VerticesOutputPropertyId = Shader.PropertyToID("verticesOutput"),
            UVOutputPropertyId = Shader.PropertyToID("uvOutput"),
            TriangleOutputPropertyId = Shader.PropertyToID("triangleOutput"),
            TimePropertyId = Shader.PropertyToID("time"),
            ScalingPropertyId = Shader.PropertyToID("scaling"),
            MeshResolutionPropertyId = Shader.PropertyToID("mesh_resolution"),
            ChunkIdPropertyId = Shader.PropertyToID("chunkId"),
            WaveParameterPropertyId = Shader.PropertyToID("wave_params"),
            WaveAmountPropertyId = Shader.PropertyToID("num_waves"),
            WaveAmplitudeScalePropertyId = Shader.PropertyToID("wave_amplitude_scale"),
            WaveTimeShiftFactorPropertyId = Shader.PropertyToID("wave_time_shift_factor");
        
        #endregion

    private void OnEnable()
    { 
        Setup();
    }
    
    private void FixedUpdate()
    {
       UpdateGerstnerWave();
    }

    private void OnDisable()
    {
        _VerticesOutputBuffer.Dispose();
        _UVOutputBuffer.Dispose();
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
            chunkId = new Vector2(chunkRowCol, chunkRowCol)
        };

        var mesh = _MeshFilter.mesh;
        _VerticesOutputBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 3);
        _UVOutputBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 2);

        _Waves = _Wave0.GenerateWaves(_Wave1, _Wave2, _WaveAmount);

        _Shader.SetFloat(ScalingPropertyId, 10 / (float)_Resolution);
        _Shader.SetInt(MeshResolutionPropertyId, _GlobalVars.resolution);
        _Shader.SetVector(ChunkIdPropertyId, _GlobalVars.chunkId);
        _Shader.SetVectorArray(WaveParameterPropertyId, new Vector4[] { _Wave0.ToVector4() });
        
        using (_TriangleOutputBuffer = new ComputeBuffer(mesh.triangles.Length, sizeof(int)))
        {
            mesh.triangles = GetBufferData(1, _TriangleOutputBuffer, TriangleOutputPropertyId, mesh.triangles);
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
        _Shader.SetFloat(WaveTimeShiftFactorPropertyId, _WaveTimeShiftFactor);
        _Shader.SetFloat(WaveAmplitudeScalePropertyId, _WaveAmplitudeScale);
        _Shader.SetVectorArray(WaveParameterPropertyId, _Waves);
        _Shader.SetInt(WaveAmountPropertyId, _WaveAmount);        
        
        _Shader.SetBuffer(0, VerticesOutputPropertyId, _VerticesOutputBuffer);
        _Shader.SetBuffer(0, UVOutputPropertyId, _UVOutputBuffer);
        
        _Shader.Dispatch(0, 32, 1, 32);

        var verticesData = new Vector3[mesh.vertexCount];
        _VerticesOutputBuffer.GetData(verticesData);
        mesh.vertices = verticesData;

        var uvData = new Vector2[mesh.vertexCount];
        _UVOutputBuffer.GetData(uvData);
        mesh.uv = uvData;

        _MeshFilter.mesh = mesh;

        _GlobalTime += (Time.fixedDeltaTime * _WaveSpeed);
    }
}
