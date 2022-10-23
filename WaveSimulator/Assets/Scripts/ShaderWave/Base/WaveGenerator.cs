using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShaderWave
{
    public struct WaveGenerator
    {
        [Serializable]
        public struct Multiplier
        {
            public float TimeShift;
            [Range(0f, .5f)]public float AmplitudeCeiling;
            [Range(0f, .5f)]public float AmplitudeGround;
            [Range(0f, .5f)]public float WaveLengthCeiling;
            [Range(0f, .5f)]public float WaveLengthGround;
            [Range(0f, .5f)]public float XShiftCeiling;
            [Range(0f, .5f)]public float XShiftGround;
            [Range(0f, .5f)]public float ZShiftCeiling;
            [Range(0f, .5f)]public float ZShiftGround;
        }
        
        internal Wave[] Waves;

        public WaveGenerator(uint amount, Wave templateWave, Multiplier multiplier)
        {
            Waves = new Wave[amount > 1000 ? 1000 : amount];

            var amplitudeMin = templateWave.Amplitude - multiplier.AmplitudeGround;
            amplitudeMin = amplitudeMin < 0 ? 0.0001f : amplitudeMin;

            var waveLengthMin = templateWave.Wavelength - multiplier.WaveLengthGround;
            waveLengthMin = waveLengthMin < 0 ? 0.0001f : waveLengthMin;
            
            for (var i = 0; i < amount; i++)
            {
                Waves[i] = new Wave()
                {
                    Amplitude = Random.Range(amplitudeMin, templateWave.Amplitude + multiplier.AmplitudeCeiling),
                    Wavelength = Random.Range(waveLengthMin, templateWave.Wavelength + multiplier.WaveLengthCeiling),
                    X = Random.Range(
                        templateWave.X - multiplier.XShiftGround, templateWave.X + multiplier.XShiftCeiling),
                    Z = Random.Range(
                        templateWave.Z - multiplier.ZShiftGround, templateWave.Z + multiplier.ZShiftCeiling),
                    TimeShift = templateWave.TimeShift + i * multiplier.TimeShift
                };
            }
        }

        public void AddWaves(Wave wave, int factor)
        {
            var wavesList = new List<Wave>();
            wavesList.AddRange(Waves);
            for (var i = 0; i < factor; i++)
                wavesList.Add(wave);

            Waves = wavesList.ToArray();
        }
    }
}
