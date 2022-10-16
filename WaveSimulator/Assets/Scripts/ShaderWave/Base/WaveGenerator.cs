using System;
using Random = UnityEngine.Random;

namespace ShaderWave
{
    public struct WaveGenerator
    {
        [Serializable]
        public struct Multiplier
        {
            public float AmplitudeCeiling;
            public float AmplitudeGround;
            public float WaveLengthCeiling;
            public float WaveLengthGround;
            public float XShiftCeiling;
            public float XShiftGround;
            public float ZShiftCeiling;
            public float ZShiftGround;
        }
        
        internal readonly Wave[] Waves;

        public WaveGenerator(uint amount, Wave templateWave, Multiplier multiplier)
        {
            multiplier.AmplitudeCeiling = multiplier.AmplitudeCeiling == 0 ? .9f : multiplier.AmplitudeCeiling;
            multiplier.AmplitudeGround = multiplier.AmplitudeGround == 0 ? .9f : multiplier.AmplitudeGround;
            multiplier.WaveLengthCeiling = multiplier.WaveLengthCeiling == 0 ? .9f : multiplier.WaveLengthCeiling;
            multiplier.WaveLengthGround = multiplier.WaveLengthGround == 0 ? .9f : multiplier.WaveLengthGround;
            multiplier.XShiftCeiling = multiplier.XShiftCeiling == 0 ? .9f : multiplier.XShiftCeiling;
            multiplier.XShiftGround = multiplier.XShiftGround == 0 ? .9f : multiplier.XShiftGround;
            multiplier.ZShiftCeiling = multiplier.ZShiftCeiling == 0 ? .9f : multiplier.ZShiftCeiling;
            multiplier.ZShiftGround = multiplier.ZShiftGround == 0 ? .9f : multiplier.ZShiftGround;
            
            Waves = new Wave[amount > 1000 ? 1000 : amount];
            for (var i = 0; i < amount; i++)
            {
                Waves[i] = new Wave()
                {
                    Amplitude = Random.Range(
                        templateWave.Amplitude * multiplier.AmplitudeGround, templateWave.Amplitude * multiplier.AmplitudeCeiling),
                    Wavelength = Random.Range(
                        templateWave.Wavelength * multiplier.WaveLengthGround, templateWave.Wavelength * multiplier.WaveLengthCeiling),
                    X = Random.Range(
                        templateWave.X * multiplier.XShiftGround, templateWave.X * multiplier.XShiftCeiling),
                    Z = Random.Range(
                        templateWave.Z * multiplier.ZShiftGround, templateWave.Z * multiplier.ZShiftCeiling)
                };
            }
        }
    }
}
