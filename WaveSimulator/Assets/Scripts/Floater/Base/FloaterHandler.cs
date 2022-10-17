using UnityEngine;

namespace Floater
{
    public static class FloaterHandler
    {
        public static void FloaterUpdate(Floater floater, int totalFloaterCount)
        {
            floater.Rigidbody.AddForceAtPosition(
                Physics.gravity / totalFloaterCount, 
                floater.Transform.position,
                ForceMode.Acceleration);

            const int waveHeight_ = 00; // GET WAVE HEIGHT AT VERTEX X
            
            if (!(floater.Transform.position.y < waveHeight_)) return;
            
            var displacementMultiplier = 
                Mathf.Clamp01((waveHeight_ - floater.Transform.position.y) / floater.DepthBeforeSubmerged) 
                * floater.DisplacementAmount;
            
            floater.Rigidbody.AddForceAtPosition(new Vector3(0f, 
                Mathf.Abs(Physics.gravity.y) * displacementMultiplier,
                0f), floater.Transform.position, ForceMode.VelocityChange);
            
            floater.Rigidbody.AddForce(
                displacementMultiplier * -floater.Rigidbody.velocity * floater.WaterDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
            
            floater.Rigidbody.AddForce(
                displacementMultiplier * -floater.Rigidbody.angularVelocity * floater.WaterAngularDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
        }
    }
}
