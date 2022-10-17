using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderWave
{
    public class WaveManager : MonoBehaviour
    {
        [Serializable]
        internal struct ShaderTemplate
        {
            public ComputeShader Shader;
            public WaveGrid MeshGrid;
            public int Resolution;
            public float Scaling;
            public Vector3 Shift;
            public int Speed;
            [Space] public Wave Wave;
            public uint WaveAmount;
            public WaveGenerator.Multiplier _Multiplier;
        }
        
        private struct ShaderContainer
        {
            internal WaveGrid MeshGrid;
            internal ShaderWave Shader;
        }

        [SerializeField] internal ShaderTemplate[] Templates;
        private readonly List<ShaderContainer> _ShaderContainers = new List<ShaderContainer>();
        
        private void Awake()
        {
            SetupContainer();
        }

        private void FixedUpdate()
        {
            foreach (var container in _ShaderContainers)
            {
                var meshGrid = container.MeshGrid;
                var gridResolution = meshGrid.GridResolution;
                var meshResolution = meshGrid.MeshResolution;
                
                for (var j = 0; j < gridResolution; j++)
                {
                    for (var i = 0; i < gridResolution; i++)
                    {
                        var mesh = meshGrid.MeshGroup[j + i * gridResolution].mesh;
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
            {
                shaderContainer.Shader.VertexBuffer.Release();
            }
        }

        private void SetupContainer()
        {
            foreach (var shaderTemplate in Templates)
            {
                var container = new ShaderContainer()
                {
                    MeshGrid = new WaveGrid(
                        shaderTemplate.MeshGrid.MeshGroup, 
                        2, // CURRENTLY HARDCODED !!! NEEDS TO BE UPDATED MIT MESH-TABLE FROM DEPTH-CALCULATOR !!!
                        shaderTemplate.Resolution),
                    Shader = new ShaderWave(
                        shaderTemplate.Shader, 
                        shaderTemplate.Shift, 
                        shaderTemplate.Scaling, 
                        shaderTemplate.Resolution,
                        shaderTemplate.Speed)
                };
                
                ShaderWaveHandler.SetupShader(ref container.Shader);
                ShaderWaveHandler.SetupWaves(
                    new WaveGenerator(shaderTemplate.WaveAmount, shaderTemplate.Wave, shaderTemplate._Multiplier), 
                    ref container.Shader);
                
                foreach (var meshGroup in container.MeshGrid.MeshGroup)
                {
                    var mesh = meshGroup.mesh;
                    ShaderWaveHandler.SetupMesh(ref mesh, container.Shader, new Vector3(
                            mesh.bounds.max.x * container.MeshGrid.GridResolution * container.MeshGrid.MeshResolution,
                            mesh.bounds.max.y,
                            mesh.bounds.max.z * container.MeshGrid.GridResolution * container.MeshGrid.MeshResolution)
                        );
                }
                
                _ShaderContainers.Add(container);
            }
        }
    }
}
