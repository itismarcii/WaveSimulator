using ShaderWave;
using UnityEngine;

namespace Floater
{
    public static class FloaterHandler
    {
        public static void FloaterUpdate(Floater floater, int totalFloaterCount, WaveGrid waveGrid)
        {
            floater.Rigidbody.AddForceAtPosition(
                Physics.gravity / totalFloaterCount, 
                floater.Transform.position,
                ForceMode.Acceleration);

            // Run twice for faster index updates (Maybe not needed !!)
            DepthCalculator.CalculateDepth(ref floater, waveGrid);
            var waveHeight = DepthCalculator.CalculateDepth(ref floater, waveGrid);

            if (!(floater.Transform.position.y < waveHeight)) return;
            
            var displacementMultiplier = 
                Mathf.Clamp01((waveHeight - floater.Transform.position.y) / floater.DepthBeforeSubmerged) 
                * floater.DisplacementAmount;
            
            floater.Rigidbody.AddForceAtPosition(new Vector3(0f, 
                Mathf.Abs(Physics.gravity.y) * displacementMultiplier,
                0f), floater.Transform.position, ForceMode.VelocityChange);
            
            floater.Rigidbody.AddForce(
                -floater.Rigidbody.velocity * (displacementMultiplier * floater.WaterDrag * Time.fixedDeltaTime),
                ForceMode.VelocityChange);
            
            floater.Rigidbody.AddForce(
                -floater.Rigidbody.angularVelocity * (displacementMultiplier * floater.WaterAngularDrag * Time.fixedDeltaTime),
                ForceMode.VelocityChange);
        }
    }
}
