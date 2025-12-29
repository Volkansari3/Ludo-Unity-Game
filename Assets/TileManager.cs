using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public List<int> safeTileIndexes = new List<int>();

    public List<Transform> tiles = new List<Transform>();

    private void Awake()
    {
        Instance = this;
        safeTileIndexes.Add(0);
        safeTileIndexes.Add(13);
        safeTileIndexes.Add(26);
        safeTileIndexes.Add(39);
    }
}

