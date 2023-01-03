using System;
using UnityEngine;

namespace ShaderWave.Base
{
    [Serializable]
    public struct WaveInformation
    {
        public Vector2 Direction;
        [Range(0,1)]public float Steepness;
        [Range(0,25)]public float WaveLength;
        [Range(.01f, 3f)]public float TimeFactor;
    }
}
