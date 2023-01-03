using System;
using UnityEngine;

namespace ShaderWave.Base
{
    [Serializable]
    public struct WaveGenerationInformation
    {
        public struct WaveGenerationInformationShaderInput
        {
            public WaveInformation[] WaveInformationArray;
            public int WaveAmount;
        }
        
        private WaveInformation[] _GeneratedWaves;

        [SerializeField] private WaveInformation _BaseWave;
    
        [SerializeField] private float
            MinX,
            MaxX,
            MinZ,
            MaxZ,
            MinAmplitude,
            MaxAmplitude,
            MinSteepness,
            MaxSteepness,
            MinWaveLength,
            MaxWaveLength;
    
        public void GenerateRandomWaves(int amount)
        {
            if (amount > 100)
            {
                Debug.LogWarning($"Max allowed amount of generated waves is 1000. 1000 Waves are now generated instead of the {amount}.");
                amount = 100;
            }
            
            _GeneratedWaves = new WaveInformation[amount];

            if (amount % 8 == 0)
            {
                GenerationLogEight(amount);
                return;
            }

            if (amount % 2 == 0)
            {
                GenerationLogTwo(amount);
                return;
            }
        
            for (var i = 0; i < amount; i++)
            {
                _GeneratedWaves[i] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };
            }
        }

        public WaveInformation[] GetGeneratedWaves() => _GeneratedWaves;

        public WaveGenerationInformationShaderInput GetShaderInput() => new WaveGenerationInformationShaderInput()
        {
            WaveInformationArray = _GeneratedWaves,
            WaveAmount = _GeneratedWaves.Length
        };

        private void GenerationLogTwo(int amount)
        {
            for (var i = 0; i < amount;)
            {
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };
            }
        }
        
        private void GenerationLogEight(int amount)
        {
            for (var i = 0; i < amount;)
            {
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };
                
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };
                
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };
                
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Amplitude = UnityEngine.Random.Range(_BaseWave.Amplitude - MinAmplitude, _BaseWave.Amplitude + MaxAmplitude),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength)
                };
            }
        }
    }
}
