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
            public MeshFilter ShaderMesh;
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
            internal Mesh ShaderMesh;
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
            foreach (var shaderContainer in _ShaderContainers)
            {
                var container = shaderContainer;
                ShaderWaveHandler.UpdateWave(ref container.ShaderMesh, container.Shader);
                container.ShaderMesh = container.ShaderMesh;
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
                    ShaderMesh = shaderTemplate.ShaderMesh.mesh,
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
                ShaderWaveHandler.SetupMesh(ref container.ShaderMesh, container.Shader);
                _ShaderContainers.Add(container);
            }
        }
    }
}
