using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Extensions;
using ShaderWave.Base;
using UnityEngine;

public class GerstnerWaveMeshManager : MonoBehaviour
{
    [SerializeField] private ComputeShader _Shader;
    [SerializeField] private MeshFilter _MeshFilter;
    [SerializeField] private WaveInformations[] _WaveInformation;

    private int _Resolution;
    private static float _Time;
    private Vector3[] _WaveTimeArray;
    private int _WaveTimeCount;

    private static bool _IsReady = false;

    private ComputeBuffer
        _VerticesOutputBuffer,
        _UVOutputBuffer,
        _TrianglesOutputBuffer,
        _WaveTimeBuffer;

    private static readonly int
        VerticesOutputPropertyId = Shader.PropertyToID("verticesOutput"),
        UVOutputPropertyId = Shader.PropertyToID("uvOutput"),
        TriangleOutputPropertyId = Shader.PropertyToID("triangleOutput"),
        TimePropertyId = Shader.PropertyToID("time"),
        ScalingPropertyId = Shader.PropertyToID("scaling"),
        MeshResolutionPropertyId = Shader.PropertyToID("mesh_resolution"),
        ChunkIdPropertyId = Shader.PropertyToID("chunkId"),
        WaveParamsArrayPropertyId = Shader.PropertyToID("wave_params_array"),
        WaveChunkArrayPropertyId = Shader.PropertyToID("wave_chunk"),
        WaveChunkArrayLengthPropertyId = Shader.PropertyToID("wave_chunk_array_length"),
        WaveTimeChunkPropertyId = Shader.PropertyToID("wave_time_chunk");

    private void OnEnable()
    {
        Setup();
    }

    private void OnDisable()
    {
        _VerticesOutputBuffer?.Dispose();
        _UVOutputBuffer?.Dispose();
        _WaveTimeBuffer?.Dispose();
    }

    private void FixedUpdate()
    {
        if(!_IsReady) return;
        
        UpdateGerstnerWave();
        // Debug.Log(_MeshFilter.mesh.vertices[55]);
    }

    private void Setup()
    {
        if(_WaveInformation.Length <= 0) return;
        
        MeshTable.SetupTable(1000);

        // Calculate basic information of the mesh
        var mesh = _MeshFilter.mesh;
        _Resolution = MeshTable.GetFraction(mesh.vertexCount);
        var chunkRowCol = _Resolution / 2 < 1 ? 1 : _Resolution / 2;

        // Set compute shader global variables 
        _Shader.SetFloat(ScalingPropertyId, 10 / (float)_Resolution);
        _Shader.SetInt(MeshResolutionPropertyId,_Resolution);
        _Shader.SetVector(ChunkIdPropertyId, new Vector2(chunkRowCol, chunkRowCol));
        
        // Generate the wave generation information input
        _WaveTimeCount = _WaveInformation.Length;
        if (_WaveInformation.Length > 10)
        {
            Debug.LogWarning($"The compute shader doesnt allow more than 10 different WaveGenerationInformation inputs. Instead of the {_WaveTimeCount} amount of WaveGenerationInformations, we locked the amount to 10.");
            _WaveTimeCount = 10;
        }

        // Generate random Waves and set them up in the compute shader 
        var waveParamsList = new List<Vector4>();
        var waveChunkArray = new Vector4[_WaveTimeCount];
        _WaveTimeArray = new Vector3[_WaveTimeCount];
        
        for (var index = 0; index < _WaveTimeCount; index++)
        {
            var waveInfo = _WaveInformation[index];
            waveInfo.GenerateRandomWaves();
            _WaveInformation[index] = waveInfo;
            
            waveParamsList.AddRange(waveInfo.GetGeneratedWaves()); 
            _WaveTimeArray[index] = new Vector4(
                0,                                                                                                    
                waveInfo.TimeFactorBase,
                waveInfo.WaveShiftBase);
            waveChunkArray[index] = new Vector2(waveInfo.WaveAmount,0);
        }

        _Shader.SetVectorArray(WaveParamsArrayPropertyId, waveParamsList.ToArray());
        _Shader.SetInt(WaveChunkArrayLengthPropertyId, _WaveTimeCount);
        _Shader.SetVectorArray(WaveChunkArrayPropertyId, waveChunkArray);
        
        // Calculate the mesh triangles via the compute shader
        using (_TrianglesOutputBuffer = new ComputeBuffer(mesh.triangles.Length, sizeof(int)))
            mesh.triangles = GetBufferData(1, _TrianglesOutputBuffer, TriangleOutputPropertyId, mesh.triangles);
        
        // Setup ComputeBuffers that will be used during the duration of the session
        _VerticesOutputBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 3);
        _UVOutputBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 2);
        _WaveTimeBuffer = new ComputeBuffer(_WaveTimeCount, sizeof(float) * 3);
        _WaveTimeBuffer.SetData(_WaveTimeArray);
        
        UpdateGerstnerWave();
        
        _IsReady = true;
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
        
        _Shader.SetFloat(TimePropertyId, _Time);
        _Shader.SetBuffer(0, VerticesOutputPropertyId, _VerticesOutputBuffer);
        _Shader.SetBuffer(0, UVOutputPropertyId, _UVOutputBuffer);
        _Shader.SetBuffer(0, WaveTimeChunkPropertyId, _WaveTimeBuffer);
        _Shader.Dispatch(0, 32,1,32);
        
        var verticesData = new Vector3[mesh.vertexCount];
        _VerticesOutputBuffer.GetData(verticesData);
        mesh.vertices = verticesData;

        var uvData = new Vector2[mesh.vertexCount];
        _UVOutputBuffer.GetData(uvData);
        mesh.uv = uvData;

        mesh.RecalculateNormals();
        _MeshFilter.mesh = mesh;
        
        for (var index = 0; index < _WaveTimeCount; index++)
        {
            var waveTime = _WaveTimeArray[index];
            waveTime.x += waveTime.y * Time.fixedDeltaTime;
            _WaveTimeArray[index] = waveTime;
        }
        
        // Debug.Log(mesh.vertices[255]);
        // Debug.Log(_WaveTimeArray[^1].x);
        _WaveTimeBuffer.SetData(_WaveTimeArray);
    }
}
