using System;
using UnityEngine;

namespace ShaderWave
{
    [Serializable]
    public struct Wave
    {
        public float X, Z;
        [Range(0f, .400f)]public float Amplitude;
        [Range(.001f, 25f)]public float Wavelength;
        public float TimeShift;
    }
}
