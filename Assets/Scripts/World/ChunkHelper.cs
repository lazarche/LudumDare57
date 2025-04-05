using UnityEngine;

public class ChunkHelper
{
    public static Vector2Int PhysicalToChunk(Vector3 position)
    {
        return PhysicalToChunk((Vector2)position);
    }
    public static Vector2Int PhysicalToChunk(Vector2 position)
    {
        Vector2Int chunkPos = new Vector2Int();
        chunkPos.x = Mathf.FloorToInt(position.x / WorldSettings.ChunkPhysicalSize);
        chunkPos.y = Mathf.FloorToInt(-position.y / WorldSettings.ChunkPhysicalSize);

        return chunkPos;
    }
    public static Vector2Int PhysicalToBlock(Vector3 position)
    {
        return PhysicalToBlock((Vector2)position);
    }
    public static Vector2Int PhysicalToBlock(Vector2 position)
    {
        Vector2Int blockPos = new Vector2Int();

        blockPos.x = Mathf.FloorToInt(position.x / WorldSettings.BlockPhysicalSize);
        blockPos.y = Mathf.FloorToInt(-position.y / WorldSettings.BlockPhysicalSize);

        blockPos.x = Mathf.FloorToInt(Mathf.Repeat(blockPos.x, WorldSettings.ChunkSize));
        blockPos.y = Mathf.FloorToInt(Mathf.Repeat(blockPos.y, WorldSettings.ChunkSize));

        blockPos.y = WorldSettings.ChunkSize - 1 - blockPos.y;

        return blockPos;
    }

}