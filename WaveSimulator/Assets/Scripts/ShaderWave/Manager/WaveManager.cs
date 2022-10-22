using System;
using System.Collections.Generic;
using System.Diagnostics;
using Extensions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ShaderWave
{
    public class WaveManager : MonoBehaviour
    {
        [Serializable]
        internal struct ShaderTemplate
        {
            public ComputeShader Shader;
            public WaveGrid MeshGrid;
            [Space(5)] public float Scaling;
            public Vector3 Shift;
            public int Speed;
            [Space(5)] public Wave Wave;
            public WaveGenerator.Multiplier _Multiplier;
            public uint WaveAmount;
        }
        
        private struct ShaderContainer
        {
            internal WaveGrid MeshGrid;
            internal ShaderWave Shader;
            internal WaveGenerator Waves;
            internal bool IsWaveDirty;

            public void UpdateWaves(WaveGenerator waveGenerator)
            {
                IsWaveDirty = true;
                Waves = waveGenerator;
            }

            public void SetupWaves()
            {
                ShaderWaveHandler.SetupWaves(Waves, ref Shader);
                IsWaveDirty = false;
            }
        }

        [SerializeField] internal ShaderTemplate[] Templates;
        private ShaderContainer[] _ShaderContainers;
        
        private void Awake()
        {
            MeshTable.SetupTable(1000);
            SetupContainer();
        }
        
        private void FixedUpdate()
        {
            foreach (var container in _ShaderContainers)
            {
                if(container.IsWaveDirty) container.SetupWaves();
                
                var meshGrid = container.MeshGrid;
                var gridResolution = meshGrid.GridResolution;
                var meshResolution = meshGrid.MeshResolution;
                
                for (var j = 0; j < gridResolution; j++)
                {
                    for (var i = 0; i < gridResolution; i++)
                    {
                        var mesh = meshGrid.MeshGroup[i + j * gridResolution].mesh;
                        ShaderWaveHandler.UpdateWave(
                            ref mesh, 
                            container.Shader,
                            new Vector2((meshResolution - 1) * i, (meshResolution - 1) * j));
                    }
                }
            }
        }

        private void OnDisable()
        {
            foreach (var shaderContainer in _ShaderContainers)
                shaderContainer.Shader.VertexBuffer.Release();
        }

        private void SetupContainer()
        {
            _ShaderContainers = new ShaderContainer[Templates.Length];

            for (var i = 0; i < Templates.Length; i++)
            {
                var shaderTemplate = Templates[i];
                var resolution = MeshTable.GetFraction(shaderTemplate.MeshGrid.MeshGroup[0].mesh.vertexCount);

                var container = new ShaderContainer()
                {
                    MeshGrid = new WaveGrid(
                        shaderTemplate.MeshGrid.MeshGroup,
                        MeshTable.GetFraction(shaderTemplate.MeshGrid.MeshGroup.Length),
                        resolution),
                    Shader = new ShaderWave(
                        shaderTemplate.Shader,
                        shaderTemplate.Shift,
                        shaderTemplate.Scaling,
                        resolution,
                        shaderTemplate.Speed),
                    Waves = new WaveGenerator(
                        shaderTemplate.WaveAmount,
                        shaderTemplate.Wave,
                        shaderTemplate._Multiplier),
                    IsWaveDirty = false
                };

                ShaderWaveHandler.SetupShader(ref container.Shader);
                ShaderWaveHandler.SetupWaves(container.Waves, ref container.Shader);

                foreach (var meshGroup in container.MeshGrid.MeshGroup)
                {
                    var mesh = meshGroup.mesh;
                    ShaderWaveHandler.SetupMesh(ref mesh, container.Shader, new Vector3(
                        mesh.bounds.max.x * container.MeshGrid.GridResolution * container.MeshGrid.MeshResolution *
                        shaderTemplate.Scaling,
                        mesh.bounds.max.y * shaderTemplate.Scaling,
                        mesh.bounds.max.z * container.MeshGrid.GridResolution * container.MeshGrid.MeshResolution *
                        shaderTemplate.Scaling)
                    );
                }

                var gridResolution = container.MeshGrid.GridResolution;

                for (var j = 0; j < gridResolution; j++)
                {
                    for (var k = 0; k < gridResolution; k++)
                    {
                        container.MeshGrid.GridPositionWorlds[k + j * gridResolution] = new Vector3(
                            k * (resolution - 1), 0, j * (resolution - 1));
                    }
                }
                
                _ShaderContainers[i] = container;
            }
        }
        
        internal WaveGrid GetWaveGrid(int index) => _ShaderContainers[index].MeshGrid;
    }
}
