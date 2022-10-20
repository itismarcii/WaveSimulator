using System;
using UnityEngine;

namespace ShaderWave
{
    [Serializable]
    public struct WaveGrid
    {
        internal int GridResolution;
        internal int MeshResolution;
        internal int CeilingStartIndex;
        internal int MeshCount;
        internal Vector3[] GridPositionWorlds;
        public MeshFilter[] MeshGroup;

        public WaveGrid(MeshFilter[] meshGroup, int gridResolution, int meshResolution)
        {
            MeshGroup = meshGroup;
            GridResolution = gridResolution;
            MeshResolution = meshResolution;
            CeilingStartIndex = MeshResolution * MeshResolution - MeshResolution;
            MeshCount = GridResolution * GridResolution;
            GridPositionWorlds = new Vector3[MeshCount];
        }
    }
}
