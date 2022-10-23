using System;
using ShaderWave;
using UnityEngine;

namespace Floater
{
    public class FloaterManager : MonoBehaviour
    {
        private int _FloaterAmount = 0;
        [SerializeField] private Floater[] _Floaters;
        [SerializeField] private WaveManager _WaveManager;
        private WaveGrid _WaveGrid;

        private void OnEnable()
        {
            _FloaterAmount = _Floaters.Length;
            _WaveGrid = _WaveManager.GetWaveGrid(0);
        }

        private void FixedUpdate()
        {
            foreach (var floater in _Floaters)
                FloaterHandler.FloaterUpdate(floater, _FloaterAmount, _WaveGrid);
        }

        internal void SetWaveGrid(WaveGrid waveGrid) => _WaveGrid = waveGrid;
    }
}
