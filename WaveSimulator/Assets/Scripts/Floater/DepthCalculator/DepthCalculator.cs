using Extensions;
using ShaderWave;
using UnityEngine;

namespace Floater
{
    public static class DepthCalculator
    {
        public static float CalculateDepth(ref Floater floater, WaveGrid grid)
        {
            var meshFilter = grid.MeshGroup[floater.GridIndex];
            var mesh = meshFilter.mesh;
            var vertexCount = mesh.vertexCount;
            var newIndex = floater.Index;
            var minDistance = Vector3.Distance(mesh.vertices[floater.Index] + meshFilter.transform.position,
                floater.Transform.position);

            var newGridIndex = floater.GridIndex;
            var index = floater.Index;
            
            CalculateMiddleRow(ref minDistance, ref newIndex, ref newGridIndex, index, mesh, meshFilter, floater, grid);

            // CalculateUpperRow(ref minDistance, ref newIndex, ref newGridIndex, mesh, meshFilter, floater, grid);
            // CalculateLowerRow(ref minDistance, ref newIndex, ref newGridIndex, mesh, meshFilter, floater, grid);

            floater.Index = newIndex;
            floater.GridIndex = newGridIndex;
            return mesh.vertices[newIndex].y;
        }

        private static int GetUpperIndex(int startIndex, int resolution) => startIndex % resolution;
        private static int GetLowerIndex(int startIndex, int resolution , int vertexCount) =>
            vertexCount - resolution + startIndex;
        private static int GetLeftIndex(int startIndex, int resolution) => --startIndex + --resolution;
        private static int GetRightIndex(int startIndex, int resolution) => ++startIndex - --resolution;

        private static void CalculateStandard(ref float minDistance,ref int newIndex, int index, Mesh mesh, Component meshFilter, Floater floater)
        {
            var distance = Vector3.Distance(mesh.vertices[index] + meshFilter.transform.position,
                floater.Transform.position);
            if (!(minDistance > distance)) return;
            minDistance = distance;
            newIndex = index;
        }

        private static void CalculateUpperRow(
            ref float minDistance,
            ref int newIndex, 
            ref int newGridIndex, 
            in Mesh mesh, 
            MeshFilter meshFilter, 
            Floater floater, 
            WaveGrid grid)
        {
            var vertexCount = mesh.vertexCount;
            var resolution = floater.MeshWidth;

            int index;
            var meshMemory = mesh;
            var meshLog = grid.MeshResolution;

            if (grid.CeilingStartIndex < floater.Index)
            {
                index = GetUpperIndex(floater.Index, resolution);
                var meshCeilingIndex = floater.GridIndex + grid.GridResolution;
                meshMemory = meshCeilingIndex < grid.MeshCount ? grid.MeshGroup[meshCeilingIndex].mesh : meshMemory;
            }
            else index = floater.Index + resolution;
            
            if (floater.Index % meshLog == 0)
            {   // INDEX IS ON BRODER LEFT
                
                if (index >= 0 && index < vertexCount) 
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);
                
                index++;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                if (floater.GridIndex % grid.GridResolution == 0) return;
                
                index = GetLeftIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                var gridIndex = floater.GridIndex - 1;
                if(gridIndex < 0) return;

                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index] + meshFilter.transform.position,
                    floater.Transform.position);
                
                if (!(minDistance > distance)) return;                
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else if ((floater.Index % meshLog) - --grid.MeshResolution == 0)
            {   // INDEX IS ON BRODER RIGHT
                
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                if ((floater.GridIndex % grid.GridResolution) - --grid.GridResolution == 0) return;
                
                index = GetRightIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                var gridIndex = floater.GridIndex + 1;
                if(gridIndex > grid.GridResolution) return;
                
                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index] + meshFilter.transform.position,
                    floater.Transform.position);
                
                if (!(minDistance > distance)) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else
            {
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                index += 2;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);
            }
        }

        private static void CalculateMiddleRow(
            ref float minDistance,
            ref int newIndex,
            ref int newGridIndex,
            int index,
            Mesh mesh,
            MeshFilter meshFilter,
            Floater floater,
            WaveGrid grid)
        {
            var vertexCount = mesh.vertexCount;
            var resolution = floater.MeshWidth;
            var meshLog = grid.MeshResolution;
            
            if (floater.Index % meshLog == 0)
            {
                // INDEX IS ON BRODER LEFT
                index++;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, mesh, meshFilter, floater);
                
                if (floater.GridIndex % grid.GridResolution == 0) return;
                
                index = GetLeftIndex(index, resolution);
                
                var gridIndex = floater.GridIndex - 1;
                if(gridIndex < 0) return;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else if ((floater.Index % meshLog) - --meshLog == 0)
            {   // INDEX IS ON BRODER RIGHT
                
                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, mesh, meshFilter, floater);

                if ((floater.GridIndex % grid.GridResolution) - --grid.GridResolution == 0) return;
                
                // Debug.Log("OLD: " + index);
                index = GetRightIndex(index, resolution);
                // Debug.Log("NEW: " + index);
                
                if (index < 0 || index >= vertexCount) return;
                var gridIndex = floater.GridIndex + 1;
                if(gridIndex > grid.GridResolution) return;
                
                var position = meshFilter.transform.position;


                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index] + grid.GridPositionWorlds[gridIndex],
                    floater.Transform.position);

                if(distance > minDistance) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else
            {
                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, mesh, meshFilter, floater);

                index += 2;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, mesh, meshFilter, floater);
            }
        }

        private static void CalculateLowerRow(
            ref float minDistance,
            ref int newIndex,
            ref int newGridIndex,
            in Mesh mesh,
            MeshFilter meshFilter,
            Floater floater,
            WaveGrid grid)
        {
            var vertexCount = mesh.vertexCount;
            var resolution = floater.MeshWidth;

            var meshMemory = mesh;
            int index;
            var meshLog = grid.MeshResolution;
            
            if (grid.MeshResolution >= floater.Index)
            {
                index = GetLowerIndex(floater.Index, resolution, vertexCount);
                var meshGroundIndex = floater.GridIndex - grid.GridResolution;
                meshMemory = meshGroundIndex >= 0 ? grid.MeshGroup[meshGroundIndex].mesh : meshMemory;
            }
            else index = floater.Index - resolution;
            
            if (floater.Index % meshLog == 0)
            {   // INDEX IS ON BRODER LEFT
                
                if (index >= 0 && index < vertexCount) 
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);
                
                index++;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                if (floater.GridIndex % grid.GridResolution == 0) return;
                
                index = GetLeftIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                var gridIndex = floater.GridIndex - 1;
                if(gridIndex < 0) return;

                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index] + meshFilter.transform.position,
                    floater.Transform.position);
                
                if (!(minDistance > distance)) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else if ((floater.Index % meshLog) - --meshLog == 0)
            {   // INDEX IS ON BRODER RIGHT
                
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                if ((floater.GridIndex % grid.GridResolution) - --grid.GridResolution == 0) return;
                
                index = GetRightIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                var gridIndex = floater.GridIndex + 1;
                if(gridIndex > grid.GridResolution) return;
                
                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index] + meshFilter.transform.position,
                    floater.Transform.position);
                
                if (!(minDistance > distance)) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else
            {
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);

                index += 2;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(ref minDistance, ref newIndex, index, meshMemory, meshFilter, floater);
            }
        }
    }
}
