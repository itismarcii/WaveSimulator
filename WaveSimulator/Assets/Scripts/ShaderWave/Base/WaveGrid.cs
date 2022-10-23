using System;
using UnityEngine;

namespace ShaderWave
{
    [Serializable]
    public struct WaveGrid
    {
        internal int GridResolution, MeshResolution, CeilingStartIndex, MeshCount;
        internal Vector3[] GridPositionWorlds;
        public MeshFilter[] MeshGroup;

        public WaveGrid(MeshFilter[] meshGroup, int gridResolution, int meshResolution)
        {
            MeshGroup = meshGroup;
            GridResolution = gridResolution;
            MeshResolution = meshResolution;
            CeilingStartIndex = (MeshResolution * MeshResolution) - MeshResolution;
            MeshCount = GridResolution * GridResolution;
            GridPositionWorlds = new Vector3[MeshCount];
        }
    }
}
