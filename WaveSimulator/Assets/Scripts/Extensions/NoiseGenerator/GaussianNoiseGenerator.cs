using System;
using UnityEngine;

public class GaussianNoiseGenerator : MonoBehaviour
{
    public float averageDistribution = 0.0f;
    public float standardDeviation = 1.0f;
    
    public Vector2[] GenerateGaussianNoiseVector2Array(int vertexCount)
    {
        var array = new Vector2[vertexCount];
        var rand = new System.Random();

        for (var i = 0; i < vertexCount; i++)
        {
            var x = (float)rand.NextGaussian(averageDistribution, standardDeviation);
            var y = (float)rand.NextGaussian(averageDistribution, standardDeviation);
            array[i] = new Vector2(x, y);
        }

        return array;
    }
}

public static class ExtensionMethods
{
    public static double NextGaussian(this System.Random rng, double mean, double stdDev)
    {
        var u1 = rng.NextDouble();
        var u2 = rng.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                            Math.Sin(2.0 * Math.PI * u2);
        var randNormal =
            mean + stdDev * randStdNormal;
        return randNormal;
    }
}
