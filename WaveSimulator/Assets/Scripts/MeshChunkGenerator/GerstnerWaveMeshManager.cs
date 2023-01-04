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
    [SerializeField] private WaveGenerationInformation[] _WaveGenerationInformation;

    private int _Resolution;
    private static float _Time;

    private ComputeBuffer
        _VerticesOutputBuffer,
        _UVOutputBuffer,
        _TrianglesOutputBuffer;

    private static readonly int
        VerticesOutputPropertyId = Shader.PropertyToID("verticesOutput"),
        UVOutputPropertyId = Shader.PropertyToID("uvOutput"),
        TriangleOutputPropertyId = Shader.PropertyToID("triangleOutput"),
        TimePropertyId = Shader.PropertyToID("time"),
        ScalingPropertyId = Shader.PropertyToID("scaling"),
        MeshResolutionPropertyId = Shader.PropertyToID("mesh_resolution"),
        ChunkIdPropertyId = Shader.PropertyToID("chunkId"),
        WaveInformationPropertyId = Shader.PropertyToID("wave_information"),
        WaveStaticValuesPropertyId = Shader.PropertyToID("wave_static_values"),
        WaveStaticValuesNumPropertyId = Shader.PropertyToID("wave_static_values_num");

    private void OnEnable()
    {
        Setup();
        UpdateGerstnerWave();
    }

    private void OnDisable()
    {
        _VerticesOutputBuffer.Dispose();
        _UVOutputBuffer.Dispose();
    }

    private void FixedUpdate()
    {
        UpdateGerstnerWave();
        // Debug.Log(_MeshFilter.mesh.vertices[55]);
    }

    private void Setup()
    {
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
        var shaderInputLength = _WaveGenerationInformation.Length;
        if (_WaveGenerationInformation.Length > 10)
        {
            Debug.LogWarning($"The compute shader doesnt allow more than 10 different WaveGenerationInformation inputs. Instead of the {shaderInputLength} amount of WaveGenerationInformations, we locked the amount to 10.");
            shaderInputLength = 10;
        }

        // Generate random Waves and set them up in the compute shader 
        var waveInformationAmount = shaderInputLength;
        var waveInformationList = new List<Vector4>();
        var waveStaticValuesArray = new Vector4[waveInformationAmount];
        for (var index = 0; index < _WaveGenerationInformation.Length; index++)
        {
            var waveGenerationInformation = _WaveGenerationInformation[index];
            waveGenerationInformation.GenerateRandomWaves();
            waveInformationList.AddRange(waveGenerationInformation.GetGeneratedWaves());
            waveStaticValuesArray[index] = new Vector4(waveGenerationInformation.WaveAmount,
                waveGenerationInformation.TimeFactorBase);
        }

        _Shader.SetVectorArray(WaveInformationPropertyId, waveInformationList.ToArray());
        _Shader.SetInt(WaveStaticValuesNumPropertyId, waveStaticValuesArray.Length);
        _Shader.SetVectorArray(WaveStaticValuesPropertyId, waveStaticValuesArray);

        // Calculate the mesh triangles via the compute shader
        using (_TrianglesOutputBuffer = new ComputeBuffer(mesh.triangles.Length, sizeof(int)))
        {
            mesh.triangles = GetBufferData(1, _TrianglesOutputBuffer, TriangleOutputPropertyId, mesh.triangles);
        }
        
        // Setup ComputeBuffers that will be used during the duration of the session
        _VerticesOutputBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 3);
        _UVOutputBuffer = new ComputeBuffer(mesh.vertexCount, sizeof(float) * 2);
        
        UpdateGerstnerWave();
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
        _Shader.Dispatch(0, 32,1,32);
        
        var verticesData = new Vector3[mesh.vertexCount];
        _VerticesOutputBuffer.GetData(verticesData);
        mesh.vertices = verticesData;

        var uvData = new Vector2[mesh.vertexCount];
        _UVOutputBuffer.GetData(uvData);
        mesh.uv = uvData;

        _MeshFilter.mesh = mesh;

        _Time += Time.fixedDeltaTime; }
}
