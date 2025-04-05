using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    Dictionary<int, Block> blocks = new Dictionary<int, Block>();
    private void Awake()
    {
        foreach (Block block in Resources.LoadAll<Block>("Blocks"))
        {
            blocks.Add(block.id, block);
            block.LoadColors();
        }

        Debug.Log("Loaded " + blocks.Count + " blocks");
    }

    public void BuildChunk(Chunk chunk)
    {
        int chunkTextureSize = WorldSettings.ChunkSize * WorldSettings.BlockResolution;

        for (int x = 0; x < WorldSettings.ChunkSize; x++)
        {
            for (int y = 0; y < WorldSettings.ChunkSize; y++)
            {
                Color[] color = GetBlock(chunk.blocks[x, y]).GetSprite();

                for (int i = 0; i < color.Length; i++)
                {
                    int row = i % WorldSettings.BlockResolution;
                    int column = i / WorldSettings.BlockResolution;

                    int globalX = x * WorldSettings.BlockResolution + row;
                    int globalY = y * WorldSettings.BlockResolution + column;

                    int index = globalX + globalY * chunkTextureSize;
                    chunk.texture[index] = color[i];
                }
            }
        }
    }


    public Block GetBlock(byte blockId)
    {
        return blocks[blockId];
    }
}
