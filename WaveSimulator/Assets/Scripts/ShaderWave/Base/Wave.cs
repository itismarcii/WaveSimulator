using System;
using UnityEngine;

namespace ShaderWave
{
    [Serializable]
    public struct Wave
    {
        public float X, Z;
        [Range(0f, 5.0000f)]public float Amplitude;
        [Range(.001f, 100)]public float Wavelength;
    }
}
