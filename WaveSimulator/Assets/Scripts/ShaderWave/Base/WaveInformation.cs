using System;
using UnityEngine;

namespace ShaderWave.Base
{
    [Serializable]
    public struct WaveInformation
    {
        public Vector2 Direction;
        [Range(0,2)]public float Steepness;
        [Range(0,100)]public float WaveLength;
        [Range(.0001f, 25f)] public float TimeFactor;
        [Range(0, 20)] public float WaveShift;
    }
}