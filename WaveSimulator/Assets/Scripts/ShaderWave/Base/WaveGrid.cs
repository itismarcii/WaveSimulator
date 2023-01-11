using System;
using Extensions;
using UnityEngine;

namespace ShaderWave
{
    [Serializable]
    public struct WaveGrid
    {
        internal int GridResolution, MeshResolution, CeilingStartIndex, MeshCount;
        internal Vector3[] GridPositionWorlds;
        [SerializeField] private MeshFilter[] MeshGroup;

        public WaveGrid(MeshFilter[] meshGroup)
        {
            MeshGroup = meshGroup;
            GridResolution = MeshTable.GetFraction(MeshGroup.Length);
            MeshResolution = MeshTable.GetFraction(meshGroup[0].mesh.vertexCount);
            CeilingStartIndex = (MeshResolution * MeshResolution) - MeshResolution;
            MeshCount = meshGroup[0].mesh.vertexCount;
            GridPositionWorlds = new Vector3[MeshCount];
        }

        public Mesh GetMesh(int index) => MeshGroup[index].mesh;
        public MeshFilter[] GetMeshGroup() => MeshGroup;
    }
}
