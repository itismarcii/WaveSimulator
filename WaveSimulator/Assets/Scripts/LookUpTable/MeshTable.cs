using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshTable
{
    private static int[] _Table = new int[] { };

    public static void SetupTable(int amount)
    {
        _Table = new int[amount];
        
        for (var i = 0; i < amount; i++)
        {
            _Table[i] = i * i;
        }
    }

    public static int GetFraction(int verticesCount)
    {
        if (_Table.Length == 0)
        {
            SetupTable(100);
            Debug.LogWarning("Please setup MeshTable before searching for the fraction.");
        }
        
        for (var i = 0; i < _Table.Length; i++)
        {
            if (_Table[i] == verticesCount) return i;
        }

        Debug.LogError("VerticesCount not inside the table. Make sure its a square number (Example: 4x4)");
        return 0;
    }
}
