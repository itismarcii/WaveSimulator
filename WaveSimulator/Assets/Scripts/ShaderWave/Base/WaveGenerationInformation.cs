using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShaderWave.Base
{
    [Serializable]
    public struct WaveGenerationInformation
    {
        private WaveInformation[] _GeneratedWaves;

        public int WaveAmount;
        [Space,SerializeField] private WaveInformation _BaseWave;
        public float TimeFactorBase { get; private set; }
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

        public Vector4[] GetGeneratedWaves()
        {
            var arrayList = new Vector4[_GeneratedWaves.Length];

            for (var i = 0; i < _GeneratedWaves.Length; i++)
            {
                var template = _GeneratedWaves[i];
                
                arrayList[i] = new Vector4(
                    template.Direction.x,
                    template.Direction.y,
                    template.Steepness,
                    template.WaveLength
                );
            }

            return arrayList.ToArray();
        }
        
        public void GenerateRandomWaves()
        {
            switch (WaveAmount)
            {
                case > 100:
                    Debug.LogWarning($"Max allowed amount of generated waves is 100. 100 Waves are now generated instead of the {WaveAmount}.");
                    WaveAmount = 100;
                    break;
                case 0:
                    WaveAmount = 1;
                    break;
            }
            
            _GeneratedWaves = new WaveInformation[WaveAmount];
            TimeFactorBase = _BaseWave.TimeFactor;

            if (WaveAmount % 8 == 0)
            {
                GenerationLogEight();
                return;
            }

            if (WaveAmount % 2 == 0)
            {
                GenerationLogTwo();
                return;
            }
        
            for (var i = 0; i < WaveAmount; i++)
            {
                _GeneratedWaves[i] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };
            }
        }
        
        private void GenerationLogTwo()
        {
            for (var i = 0; i < WaveAmount;)
            {
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };
            }
        }
        
        private void GenerationLogEight()
        {
            for (var i = 0; i < WaveAmount;)
            {
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };
                
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };
                
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };
                
                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };

                _GeneratedWaves[i++] = new WaveInformation
                {
                    Direction = new Vector2(
                        UnityEngine.Random.Range(_BaseWave.Direction.x - MinX, _BaseWave.Direction.x + MaxX),
                        UnityEngine.Random.Range(_BaseWave.Direction.y - MinZ, _BaseWave.Direction.y + MaxZ)
                    ),
                    Steepness = UnityEngine.Random.Range(_BaseWave.Steepness - MinSteepness, _BaseWave.Steepness + MaxSteepness),
                    WaveLength = UnityEngine.Random.Range(_BaseWave.WaveLength - MinWaveLength, _BaseWave.WaveLength + MaxWaveLength),
                    TimeFactor = _BaseWave.TimeFactor
                };
            }
        }
    }
}
