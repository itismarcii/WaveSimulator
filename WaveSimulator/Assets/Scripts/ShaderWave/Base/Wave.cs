using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


namespace ShaderWave
{
    [Serializable]
    public struct Wave
    {
        private const float MAX_STEEPNESS_CLAMP = 2f;
        private const float MIN_STEEPNESS_CLAMP = .001f;
        private const float MAX_WAVELENGTH_CLAMP = 25f;
        private const float MIN_WAVELENGTH_CLAMP = .001f;

        public float X, Z;
        [Range(.001f, 1)] public float Steepness;
        [Range(.001f, 25), Space]public float Wavelength;
        public float MinX, MaxX, MinZ, MaxZ, MaxWavelength, MinWavelength;
        [Range(0.001f, .2f)] public float MaxSteepness, MinSteepness;

        public Vector4 ToVector4() => new Vector4(X, Z, Steepness, Wavelength);

        public Vector4[] GenerateWaves(Wave extraWave0, Wave extraWave1, int amount)
        {
            var waves = new Vector4[amount];
            
            if(0 != amount % 3) Debug.LogWarning("Please use only amount dividable by 3");
            
            for (var i = 0; i < amount; i += 3)
            {
                var x = UnityEngine.Random.Range(X - MinX, X + MaxX);
                var z = UnityEngine.Random.Range(Z - MinZ, Z + MaxZ);
                var steepness = Mathf.Clamp(UnityEngine.Random.Range(Steepness - MinSteepness, Steepness + MaxSteepness), MIN_STEEPNESS_CLAMP, MAX_STEEPNESS_CLAMP);
                var wavelength = Mathf.Clamp(UnityEngine.Random.Range(Wavelength - MinWavelength, Wavelength + MaxWavelength), MIN_WAVELENGTH_CLAMP, MAX_WAVELENGTH_CLAMP);
                
                waves[i] = new Vector4()
                {
                    x = x,
                    y = z,
                    z = steepness,
                    w = wavelength
                };
                
                x = UnityEngine.Random.Range(extraWave0.X - extraWave0.MinX, extraWave0.X + extraWave0.MaxX);
                z = UnityEngine.Random.Range(extraWave0.Z - extraWave0.MinZ, extraWave0.Z + extraWave0.MaxZ);
                steepness = Mathf.Clamp(UnityEngine.Random.Range(extraWave0.Steepness - extraWave0.MinSteepness,extraWave0.Steepness + extraWave0.MaxSteepness), MIN_STEEPNESS_CLAMP, MAX_STEEPNESS_CLAMP);
                wavelength = Mathf.Clamp(UnityEngine.Random.Range(extraWave0.Wavelength - extraWave0.MinWavelength,extraWave0.Wavelength + extraWave0.MaxWavelength), MIN_WAVELENGTH_CLAMP, MAX_WAVELENGTH_CLAMP);
                
                waves[i + 1] = new Vector4()
                {
                    x = x,
                    y = z,
                    z = steepness,
                    w = wavelength
                };

                x = UnityEngine.Random.Range(extraWave1.X - extraWave1.MinX, extraWave1.X + extraWave1.MaxX);
                z = UnityEngine.Random.Range(extraWave1.Z - extraWave1.MinZ, extraWave1.Z + extraWave1.MaxZ);
                steepness = Mathf.Clamp(UnityEngine.Random.Range(extraWave1.Steepness - extraWave1.MinSteepness,extraWave1.Steepness + extraWave1.MaxSteepness), MIN_STEEPNESS_CLAMP, MAX_STEEPNESS_CLAMP);
                wavelength = Mathf.Clamp(UnityEngine.Random.Range(extraWave1.Wavelength - extraWave1.MinWavelength,extraWave1.Wavelength + extraWave1.MaxWavelength), MIN_WAVELENGTH_CLAMP, MAX_WAVELENGTH_CLAMP);
                
                waves[i + 2] = new Vector4()
                {
                    x = x,
                    y = z,
                    z = steepness,
                    w = wavelength
                };
            }

            return waves;
        }
    }
}
