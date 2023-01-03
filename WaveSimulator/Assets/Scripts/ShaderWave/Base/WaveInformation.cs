using System;
using UnityEngine;

namespace ShaderWave.Base
{
    [Serializable]
    public struct WaveInformation
    {
        public Vector2 Direction;
        public float Amplitude;
        public float Steepness;
        public float WaveLength;
    }
}
