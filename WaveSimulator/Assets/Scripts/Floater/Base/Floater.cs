using System;
using Extensions;
using ShaderWave;
using UnityEngine;

namespace Floater
{
    [Serializable]
    public struct Floater
    {
        public int Index { get; internal set; }
        public int GridIndex { get; internal set; }
        public int MeshWidth { get; private set; }
        public Transform Transform;
        public Rigidbody Rigidbody;
        public float DepthBeforeSubmerged;
        public float DisplacementAmount; 
        public float WaterDrag; 
        public float WaterAngularDrag;

        public Floater(Transform transform, Rigidbody rigidbody, float depthBeforeSubmerged, float displacementAmount, float waterDrag, float waterAngularDrag)
        {
            Transform = transform;
            Rigidbody = rigidbody;
            DepthBeforeSubmerged = depthBeforeSubmerged;
            DisplacementAmount = displacementAmount;
            WaterDrag = waterDrag;
            WaterAngularDrag = waterAngularDrag;
            Index = 0;
            MeshWidth = 0;
            GridIndex = 0;
        }

        public void SetMeshIndex(WaveGrid waveGrid, int index, bool updateMeshWith = false)
        {
            GridIndex = index;
            if(updateMeshWith || MeshWidth == 0) MeshWidth = MeshTable.GetFraction(waveGrid.GetMesh(index).vertexCount);
        }
    }
}
