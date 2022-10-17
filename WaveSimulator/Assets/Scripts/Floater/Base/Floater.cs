using System;
using UnityEngine;

namespace Floater
{
    [Serializable]
    public struct Floater
    {
        public int Index;
        public Mesh CurrentMesh { get; private set; }
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
            CurrentMesh = null; // GET ACTUAL MESH !!!
            MeshWidth = 0;
        }

        public void SetMesh(Mesh mesh)
        {
            CurrentMesh = mesh;
            MeshWidth = MeshTable.GetFraction(mesh.vertexCount);
        }
    }
}
