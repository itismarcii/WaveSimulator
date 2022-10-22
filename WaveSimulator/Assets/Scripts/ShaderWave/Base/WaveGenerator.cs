using System;
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
            [Range(0f, .3f)]public float AmplitudeCeiling;
            [Range(0f, .3f)]public float AmplitudeGround;
            [Range(0f, .3f)]public float WaveLengthCeiling;
            [Range(0f, .3f)]public float WaveLengthGround;
            [Range(0f, .3f)]public float XShiftCeiling;
            [Range(0f, .3f)]public float XShiftGround;
            [Range(0f, .3f)]public float ZShiftCeiling;
            [Range(0f, .3f)]public float ZShiftGround;
        }
        
        internal readonly Wave[] Waves;

        public WaveGenerator(uint amount, Wave templateWave, Multiplier multiplier)
        {
            Waves = new Wave[amount > 1000 ? 1000 : amount];
            for (var i = 0; i < amount; i++)
            {
                Waves[i] = new Wave()
                {
                    Amplitude = Random.Range(
                        templateWave.Amplitude - multiplier.AmplitudeGround, templateWave.Amplitude + multiplier.AmplitudeCeiling),
                    Wavelength = Random.Range(
                        templateWave.Wavelength - multiplier.WaveLengthGround, templateWave.Wavelength + multiplier.WaveLengthCeiling),
                    X = Random.Range(
                        templateWave.X - multiplier.XShiftGround, templateWave.X + multiplier.XShiftCeiling),
                    Z = Random.Range(
                        templateWave.Z - multiplier.ZShiftGround, templateWave.Z + multiplier.ZShiftCeiling),
                    TimeShift = templateWave.TimeShift + i * multiplier.TimeShift
                };
            }
        }
    }
}
