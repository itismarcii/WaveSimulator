using System;
using UnityEngine;

namespace Floater
{
    [Serializable]
    public struct Floater
    {
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
        }
    }
}
