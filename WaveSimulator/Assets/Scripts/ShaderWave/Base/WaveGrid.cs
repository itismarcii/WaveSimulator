using System;
using UnityEngine;

namespace ShaderWave
{
    [Serializable]
    public struct WaveGrid
    {
        internal int GridResolution;
        internal int MeshResolution;
        public MeshFilter[] MeshGroup;

        public WaveGrid(MeshFilter[] meshGroup, int gridResolution, int meshResolution)
        {
            MeshGroup = meshGroup;
            GridResolution = gridResolution;
            MeshResolution = meshResolution;
        }
    }
}
