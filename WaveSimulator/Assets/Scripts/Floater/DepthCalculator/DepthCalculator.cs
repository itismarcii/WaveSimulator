using UnityEngine;

namespace Floater
{
    public static class DepthCalculator
    {
        public static float CalculateDepth(in Mesh mesh, ref Floater floater)
        {
            var width = floater.MeshWidth;
            var vertexCount = mesh.vertexCount;
            var minDistance = float.MaxValue;
            var newIndex = floater.Index;
            
            
            var index = floater.Index + width - 1;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }

            index++;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }
            
            index++;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }

            index = floater.Index - 1;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }
            
            index = floater.Index + 1;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }

            index = floater.Index - width - 1;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }

            index++;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }

            index++;
            if (index > 0 && index < vertexCount)
            {
                var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    newIndex = index;
                }
            }

            floater.Index = newIndex;
            return mesh.vertices[newIndex].y;
        }
    }
}
