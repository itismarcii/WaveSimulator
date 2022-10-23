using Extensions;
using ShaderWave;
using UnityEngine;
using UnityEngine.AI;

namespace Floater
{
    public static class DepthCalculator
    {
        public static float CalculateDepth(ref Floater floater, WaveGrid grid)
        {
            var newIndex = floater.Index;
            var newGridIndex = floater.GridIndex;
            var minDistance = Vector3.Distance(grid.MeshGroup[newGridIndex].mesh.vertices[newIndex] + grid.GridPositionWorlds[newGridIndex],
                floater.Transform.position);
            
            CalculateMiddleRow(ref minDistance, ref newIndex, ref newGridIndex, newIndex, grid.MeshGroup[newGridIndex].mesh, floater, grid);
            CalculateUpperRow(ref minDistance, ref newIndex, ref newGridIndex, grid.MeshGroup[newGridIndex].mesh , floater, grid);
            CalculateLowerRow(ref minDistance, ref newIndex, ref newGridIndex, grid.MeshGroup[newGridIndex].mesh, floater, grid);
            
            floater.Index = newIndex;
            floater.GridIndex = newGridIndex;
            return grid.MeshGroup[floater.GridIndex].mesh.vertices[newIndex].y;
        }

        private static int GetUpperIndex(int startIndex, int resolution) => startIndex % resolution;
        private static int GetLowerIndex(int startIndex, int resolution , int vertexCount) =>
            vertexCount - resolution + startIndex;
        private static int GetLeftIndex(int startIndex, int resolution) => --startIndex + --resolution;
        private static int GetRightIndex(int startIndex, int resolution) => ++startIndex - --resolution;

        private static void CalculateStandard(ref float minDistance,ref int newIndex, int index, Mesh mesh, Floater floater)
        {
            var distance = Vector3.Distance(mesh.vertices[index], floater.Transform.position);
            if (!(minDistance > distance)) return;
            minDistance = distance;
            newIndex = index;
        }

        private static void CalculateUpperRow(
            ref float minDistance,
            ref int newIndex, 
            ref int newGridIndex, 
            Mesh mesh, 
            Floater floater, 
            WaveGrid grid)
        {
            var vertexCount = mesh.vertexCount;
            var resolution = floater.MeshWidth;

            var index = floater.Index + resolution;
            var meshLog = grid.MeshResolution;
            var gridIndex = newGridIndex;
            var distanceMemory = minDistance;
            
            if (grid.CeilingStartIndex < index)
            {
                var meshCeilingIndex = floater.GridIndex + grid.GridResolution;
                if (meshCeilingIndex > 0 && meshCeilingIndex < grid.MeshCount)
                {
                    index = GetUpperIndex(floater.Index, resolution);
                    gridIndex = meshCeilingIndex;
                    mesh = grid.MeshGroup[meshCeilingIndex].mesh;
                }
            }
            
            if (floater.Index % meshLog == 0)
            {   // INDEX IS ON BRODER LEFT
                
                if (index >= 0 && index < vertexCount) 
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);
                
                index++;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                if (floater.GridIndex % grid.GridResolution == 0) return;
                
                index = GetLeftIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                gridIndex--;
                if(gridIndex < 0) return;

                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index],
                    floater.Transform.position);
                
                if(distance > minDistance) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else if ((floater.Index % meshLog) - --grid.MeshResolution == 0)
            {   // INDEX IS ON BRODER RIGHT
                
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                if ((floater.GridIndex % grid.GridResolution) - --grid.GridResolution == 0) return;
                
                index = GetRightIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                gridIndex++;
                if(gridIndex >= grid.MeshCount) return;
                
                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index],
                    floater.Transform.position);
                
                if(distance > minDistance) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else
            {
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index += 2;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                if(minDistance < distanceMemory) newGridIndex = gridIndex;
            }
        }

        private static void CalculateMiddleRow(
            ref float minDistance,
            ref int newIndex,
            ref int newGridIndex,
            int index,
            Mesh mesh,
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
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);
                
                if (floater.GridIndex % grid.GridResolution == 0) return;
                
                index = GetLeftIndex(index, resolution);
                
                var gridIndex = floater.GridIndex - 1;
                if(gridIndex < 0) return;
                
                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index],
                    floater.Transform.position);
                
                if(distance > minDistance) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else if ((floater.Index % meshLog) - --meshLog == 0)
            {   // INDEX IS ON BRODER RIGHT
                
                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                if ((floater.GridIndex % grid.GridResolution) - --grid.GridResolution == 0) return;
                
                index = GetRightIndex(index, resolution);
                
                if (index < 0 || index >= vertexCount) return;
                var gridIndex = floater.GridIndex + 1;
                if(gridIndex >= grid.MeshCount) return;
                
                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index],
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
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index += 2;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);
            }
        }

        private static void CalculateLowerRow(
            ref float minDistance,
            ref int newIndex,
            ref int newGridIndex,
            Mesh mesh,
            Floater floater,
            WaveGrid grid)
        {
            var vertexCount = mesh.vertexCount;
            var resolution = floater.MeshWidth;

            var index = floater.Index - resolution;
            var meshLog = grid.MeshResolution;
            var gridIndex = newGridIndex;
            var distanceMemory = minDistance;
            
            if (grid.MeshResolution >= index)
            {                
                var meshGroundIndex = floater.GridIndex - grid.GridResolution;
                if(meshGroundIndex >= 0 && meshGroundIndex < grid.MeshCount)
                {               
                    index = GetLowerIndex(-floater.Index, resolution, vertexCount);
                    gridIndex = meshGroundIndex;
                    mesh = grid.MeshGroup[meshGroundIndex].mesh;
                }
            }


            if (floater.Index % meshLog == 0)
            {   // INDEX IS ON BRODER LEFT
                
                if (index >= 0 && index < vertexCount) 
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);
                
                index++;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                if (floater.GridIndex % grid.GridResolution == 0) return;
                
                index = GetLeftIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                gridIndex--;
                if(gridIndex < 0) return;

                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index],
                    floater.Transform.position);
                
                if(distance > minDistance) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else if ((floater.Index % meshLog) - --meshLog == 0)
            {   // INDEX IS ON BRODER RIGHT
                
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                if ((floater.GridIndex % grid.GridResolution) - --grid.GridResolution == 0) return;
                
                index = GetRightIndex(index, resolution);
                if (index < 0 || index >= vertexCount) return;

                gridIndex++;
                if(gridIndex >= grid.MeshCount) return;
                
                var distance = Vector3.Distance(
                    grid.MeshGroup[gridIndex].mesh.vertices[index],
                    floater.Transform.position);
                
                if(distance > minDistance) return;
                minDistance = distance;
                newIndex = index;
                newGridIndex = gridIndex;
            }
            else
            {
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index--;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);

                index += 2;
                if (index >= 0 && index < vertexCount)
                    CalculateStandard(
                        ref minDistance, ref newIndex, index, mesh, floater);
                
                if(minDistance < distanceMemory) newGridIndex = gridIndex;
            }
        }
    }
}
