using UnityEngine;

namespace ShaderWave
{
    public struct ShaderWave
    {
        internal readonly ComputeShader Shader;
        internal readonly ComputeBuffer VertexBuffer;
        internal Vector3 Shift;
        internal readonly float Scaling;
        internal readonly int ResolutionId, TimeId, VertexBufferId, ShiftId, ScalingId;
        internal readonly int Resolution;
        internal int[] KernelInformation;
        internal readonly float Speed;

        public ShaderWave(ComputeShader shader, Vector3 shift, float scaling, int resolution, float speed = 1) : this()
        {
            Shader = shader;
            Shift = shift;
            Scaling = scaling;
            Resolution = resolution;
            Speed = speed;
        
            VertexBufferId = UnityEngine.Shader.PropertyToID("vertices");
            ResolutionId = UnityEngine.Shader.PropertyToID("resolution");
            TimeId = UnityEngine.Shader.PropertyToID("time");
            ShiftId = UnityEngine.Shader.PropertyToID("shift");
            ScalingId = UnityEngine.Shader.PropertyToID("scaling");

            var count = Resolution * Resolution;
            VertexBuffer = new ComputeBuffer(count, sizeof(float) * 3);
        }
        
        public void SetKernelInfo(int index, int x, int y, int z) => KernelInformation = new int[] { index, x, y, z };

    }
}
