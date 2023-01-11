using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Extensions;
using ShaderWave;
using ShaderWave.Base;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GerstnerWaveMeshManager : MonoBehaviour
{
    [SerializeField] private ComputeShader _Shader;
    [SerializeField] private float _Scaling = 1;
    private int _GridResolution;
    [SerializeField] private GridHolder _GridHolder;
    private WaveGrid _GridMesh;
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
        GridShiftPropertyId = Shader.PropertyToID("grid_shift"),
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

        GridUpdate();
    }

    private void Setup()
    {
        if(_GridHolder == null) return;
        
        MeshTable.SetupTable(1000);
        _GridMesh = _GridHolder.Setup();
        
        if(_GridMesh.GetMeshGroup().Length <= 0) return;
        
        _GridResolution = _GridMesh.GridResolution;
        _Resolution = _GridMesh.MeshResolution;

        // Calculate mesh chunk resolution
        var chunkResolution = _Resolution / 2 < 1 ? 1 : _Resolution / 2;
        if(_WaveInformation.Length <= 0) return;
        
        // Set compute shader global variables 
        if(_Scaling <= 0) return;
        _Shader.SetFloat(ScalingPropertyId, (10 / (float)_Resolution) * _Scaling);
        _Shader.SetInt(MeshResolutionPropertyId,_Resolution);
        _Shader.SetVector(ChunkIdPropertyId, new Vector2(chunkResolution, chunkResolution));
        
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
        
        // Setup ComputeBuffers that will be used during the duration of the session
        _VerticesOutputBuffer = new ComputeBuffer(_GridMesh.MeshCount, sizeof(float) * 3);
        _UVOutputBuffer = new ComputeBuffer(_GridMesh.MeshCount, sizeof(float) * 2);
        _WaveTimeBuffer = new ComputeBuffer(_WaveTimeCount, sizeof(float) * 3);
        _WaveTimeBuffer.SetData(_WaveTimeArray);

        for (var x = 0; x < _GridResolution; x++)
        {
            for (var y = 0; y < _GridResolution; y++)
            {
                if(!MeshSetup(_GridMesh.GetMesh(x + y * _GridResolution),
                       new Vector2(x * (_GridMesh.MeshResolution - 1),y * (_GridMesh.MeshResolution - 1)))
                   ) return;
            }
        }
    }

    private void GridUpdate()
    {
        for (var x = 0; x < _GridResolution; x++)
        {
            for (var y = 0; y < _GridResolution; y++)
            {
                UpdateGerstnerWave(
                    _GridMesh.GetMesh(x + y * _GridResolution), 
                    new Vector2(x * (_GridMesh.MeshResolution - 1),y * (_GridMesh.MeshResolution - 1))
                    );
            }
        }
        
        for (var index = 0; index < _WaveTimeCount; index++)
        {
            var waveTime = _WaveTimeArray[index];
            waveTime.x += waveTime.y * Time.fixedDeltaTime;
            _WaveTimeArray[index] = waveTime;
        }
        
        _WaveTimeBuffer.SetData(_WaveTimeArray);
    }

    private bool MeshSetup(Mesh mesh, Vector2 shift)
    {
        // Calculate the mesh triangles via the compute shader
        using (_TrianglesOutputBuffer = new ComputeBuffer(mesh.triangles.Length, sizeof(int)))
            mesh.triangles = GetBufferData(1, _TrianglesOutputBuffer, TriangleOutputPropertyId, mesh.triangles);

        mesh.bounds = new Bounds(
            Vector3.zero, new Vector3(
                    mesh.bounds.max.x * _GridMesh.GridResolution * _GridMesh.MeshResolution,
                    mesh.bounds.max.y,
                    mesh.bounds.max.z * _GridMesh.GridResolution * _GridMesh.MeshResolution));

        try
        {       
            UpdateGerstnerWave(mesh, shift);
            _IsReady = true;
        }
        catch (Exception e)
        {
            _IsReady = false;
        }

        return _IsReady;
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

    private void UpdateGerstnerWave(Mesh mesh, Vector2 shift)
    {
        _Shader.SetFloat(TimePropertyId, _Time);
        _Shader.SetVector(GridShiftPropertyId, shift);
        _Shader.SetBuffer(0, VerticesOutputPropertyId, _VerticesOutputBuffer);
        _Shader.SetBuffer(0, UVOutputPropertyId, _UVOutputBuffer);
        _Shader.SetBuffer(0, WaveTimeChunkPropertyId, _WaveTimeBuffer);
        _Shader.Dispatch(0, 32, 1, 32);

        var verticesData = new Vector3[mesh.vertexCount];
        _VerticesOutputBuffer.GetData(verticesData);
        mesh.vertices = verticesData;

        var uvData = new Vector2[mesh.vertexCount];
        _UVOutputBuffer.GetData(uvData);
        mesh.uv = uvData;

        mesh.RecalculateNormals();
    }
}
