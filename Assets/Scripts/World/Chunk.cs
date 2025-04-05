using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public Vector2Int position;
    public byte[,] blocks = new byte[WorldSettings.ChunkSize, WorldSettings.ChunkSize];
    public Color[] texture = new Color[WorldSettings.ChunkSize * WorldSettings.BlockResolution * WorldSettings.ChunkSize * WorldSettings.BlockResolution];
    public List<Vector2[]> collider = new List<Vector2[]>();

    public bool IsGenerated = false;
    public ChunkWorld chunkWorld;
}
