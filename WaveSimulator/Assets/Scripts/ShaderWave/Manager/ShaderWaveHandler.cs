using UnityEngine;

namespace ShaderWave
{
    public static class ShaderWaveHandler
    {
        public static void SetupShader(ref ShaderWave shader, int kernelIndex = 0, int[] threadGroups = null)
        {
            if (threadGroups is {Length: > 3}) return;
            
            threadGroups ??= new int[] {32, 1, 32};

            shader.KernelInformation = new int[] {kernelIndex, threadGroups[0], threadGroups[1], threadGroups[2]};
            
            shader.Shader.SetInt(shader.ResolutionId, shader.Resolution);
            shader.Shader.SetFloat(shader.ScalingId, shader.Scaling);
            shader.Shader.SetVector(shader.ShiftId, shader.Shift);
            shader.SetKernelInfo(kernelIndex, threadGroups[0], threadGroups[1], threadGroups[2]);
        }

        public static void SetupMesh(ref Mesh mesh, in ShaderWave shader, Vector3 maxBound)
        {
            var kernelInfo = shader.KernelInformation;

            //UVs setup
            var count = shader.Resolution * shader.Resolution;
            var uvBuffer =  new ComputeBuffer(count, sizeof(float) * 2);
            shader.Shader.SetBuffer(1, Shader.PropertyToID("uvs"), uvBuffer);
            shader.Shader.Dispatch(1, kernelInfo[1], kernelInfo[2], kernelInfo[3]);
            
            var uvs = new Vector2[mesh.uv.Length];
            uvBuffer.GetData(uvs);
            mesh.uv = uvs;
            uvBuffer.Release();
            
            //Triangles setup
            count = (shader.Resolution - 1) * (shader.Resolution - 1);
            var triangleBuffer = new ComputeBuffer(count * 6, sizeof(int));
            shader.Shader.SetBuffer(2, Shader.PropertyToID("triangles"), triangleBuffer);
            shader.Shader.Dispatch(2, kernelInfo[1], kernelInfo[2], kernelInfo[3]);
            
            var triangles = new int[mesh.triangles.Length];
            triangleBuffer.GetData(triangles);
            mesh.triangles = triangles;
            triangleBuffer.Release();

            mesh.bounds = new Bounds(Vector3.zero, maxBound);
        }
        
        public static void SetupWaves(WaveGenerator waves, ref ShaderWave shader)
        {
            var waveArray = new Vector4[waves.Waves.Length];

            for (var i = 0; i < waveArray.Length; i++)
            {
                var wave = waves.Waves[i];
                waveArray[i] = new Vector4(wave.X, wave.Z, wave.Amplitude, wave.TimeShift);
            }
            
            shader.Shader.SetVectorArray(Shader.PropertyToID("waves"), waveArray);
            shader.Shader.SetInt(Shader.PropertyToID("waves_length"), waveArray.Length);
        }        
        
        public static void UpdateWave(ref Mesh mesh, in ShaderWave shader, Vector2 startIndex)
        {
            var kernelInfo = shader.KernelInformation;
            shader.Shader.SetFloat(shader.TimeId, Time.fixedTime * shader.Speed);
            shader.Shader.SetVector(shader.StartIndexId, startIndex);
            shader.Shader.SetBuffer(0, shader.VertexBufferId, shader.VertexBuffer);
            shader.Shader.Dispatch(
                kernelInfo[0], kernelInfo[1], kernelInfo[2], kernelInfo[3]
                );
            var vertices = new Vector3[mesh.vertexCount];
            shader.VertexBuffer.GetData(vertices);
            mesh.vertices = vertices;
        }
    }
}
