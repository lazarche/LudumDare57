using UnityEngine;

[System.Serializable]
public class Chunk
{
    public Vector2Int position;
    public byte[,] blocks = new byte[WorldSettings.ChunkSize, WorldSettings.ChunkSize];
    public Color[] texture = new Color[WorldSettings.ChunkSize * WorldSettings.BlockResolution * WorldSettings.ChunkSize * WorldSettings.BlockResolution];

    public bool IsGenerated = false;
    public ChunkWorld chunkWorld;
}
