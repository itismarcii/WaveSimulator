using System.Collections.Generic;
using Extensions;
using ShaderWave;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
    [SerializeField] private GameObject _MeshPrefab;
    [SerializeField] private uint _GridResolution = 1;

    public WaveGrid Setup()
    {
        _GridResolution = _GridResolution > 0 ? _GridResolution : 1;
        
        var meshFilters = new List<MeshFilter>();
        
        for (var i = 0; i < _GridResolution * _GridResolution; i++)
        {
            var meshFilterObj = Instantiate(_MeshPrefab, transform);
            meshFilters.Add(meshFilterObj.GetComponent<MeshFilter>());
        }
        
        return new WaveGrid(meshFilters.ToArray());
    }
}
