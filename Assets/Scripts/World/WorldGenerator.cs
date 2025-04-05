using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public void GenerateChunk(Chunk chunk)
    {
        System.Random random = new System.Random();
        for (int x = 0; x < WorldSettings.ChunkSize; x++)
        {
            for(int y = 0; y < WorldSettings.ChunkSize; y++)
            {
                chunk.blocks[x, y] = (byte) random.Next(0, 3);
            }
        }

        chunk.IsGenerated = true;
    }
}
