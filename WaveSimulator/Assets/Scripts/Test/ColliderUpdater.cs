using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderUpdater : MonoBehaviour
{
    private MeshCollider _Collider;
    private MeshFilter _MeshFilter;
    
    void Start()
    {
        _Collider = GetComponent<MeshCollider>();
        _MeshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        _Collider.sharedMesh = _MeshFilter.mesh;
    }
}
