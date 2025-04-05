using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    float[,] cachedPerlin = new float[200, 200];

    Dictionary<int, Block> blocks = new Dictionary<int, Block>();
    private void Awake()
    {
        edgeOverlays = new Dictionary<int, Color[]>();
        edgeOverlays.Add(1, Resources.Load<Texture2D>("EdgeOverlays/Top").GetPixels());
        edgeOverlays.Add(2, Resources.Load<Texture2D>("EdgeOverlays/Bottom").GetPixels());
        edgeOverlays.Add(4, Resources.Load<Texture2D>("EdgeOverlays/Left").GetPixels());
        edgeOverlays.Add(8, Resources.Load<Texture2D>("EdgeOverlays/Right").GetPixels());

        foreach (Block block in Resources.LoadAll<Block>("Blocks"))
        {
            blocks.Add(block.id, block);
            block.LoadColors();
        }

        Debug.Log("Loaded " + blocks.Count + " blocks");
        System.Random random = new System.Random();
        for (int x = 0; x < 100; x++)
            for (int y = 0; y < 100; y++)
                cachedPerlin[x, y] = (float)random.NextDouble();
    }

    public float GetPerlin(int x, int y)
    {
        x = x % 200;
        y = y % 200;
        return cachedPerlin[x, y];
    }

    public void BuildChunk(Chunk chunk)
    {
        //Collider
        chunk.collider.Clear();
        chunk.collider = GeneratePathsFromTexture(chunk);


        int chunkTextureSize = WorldSettings.ChunkSize * WorldSettings.BlockResolution;

        Chunk[] cachedAround = new Chunk[4];
        cachedAround[0] = WorldManager.Instance.GetChunk(new Vector2Int(chunk.position.x, chunk.position.y + 1));
        cachedAround[1] = WorldManager.Instance.GetChunk(new Vector2Int(chunk.position.x, chunk.position.y - 1));
        cachedAround[2] = WorldManager.Instance.GetChunk(new Vector2Int(chunk.position.x - 1, chunk.position.y));
        cachedAround[3] = WorldManager.Instance.GetChunk(new Vector2Int(chunk.position.x + 1, chunk.position.y));

        for (int x = 0; x < WorldSettings.ChunkSize; x++)
        {
            for (int y = 0; y < WorldSettings.ChunkSize; y++)
            {
                Block current = GetBlock(chunk.blocks[x, y]);
                Color[] baseColor = current.GetSprite(GetPerlin(x,y));
                BlitBlockToChunk(chunk.texture, baseColor, x, y, chunkTextureSize);

                int edgeMask = GetEdgeMask(chunk, x, y, current, cachedAround);

                if ((edgeMask & 1) != 0) BlitToChunk(chunk.texture, GetEdgeOverlay(1), x, y, chunkTextureSize); // Top
                if ((edgeMask & 2) != 0) BlitToChunk(chunk.texture, GetEdgeOverlay(2), x, y, chunkTextureSize); // Bottom
                if ((edgeMask & 4) != 0) BlitToChunk(chunk.texture, GetEdgeOverlay(4), x, y, chunkTextureSize); // Left
                if ((edgeMask & 8) != 0) BlitToChunk(chunk.texture, GetEdgeOverlay(8), x, y, chunkTextureSize); // Right
            }
        }
    }

    private int GetEdgeMask(Chunk chunk, int x, int y, Block current, Chunk[] cachedAround)
    {
        if (chunk.blocks[x, y] == 0)
            return 0;
        int mask = 0;

        if (IsAirBlock(chunk, x, y + 1, cachedAround[1])) mask |= 1;  // Top
        if (IsAirBlock(chunk, x, y - 1, cachedAround[0])) mask |= 2;  // Bottom
        if (IsAirBlock(chunk, x - 1, y, cachedAround[2])) mask |= 4;  // Left
        if (IsAirBlock(chunk, x + 1, y, cachedAround[3])) mask |= 8;  // Right

        return mask;
    }

    private bool IsAirBlock(Chunk chunk, int x, int y, Chunk cached = null)
    {
        if (x >= 0 && y >= 0 && x < WorldSettings.ChunkSize && y < WorldSettings.ChunkSize)
            return GetBlock(chunk.blocks[x, y]).id == 0;

        if (cached != null && cached.IsGenerated)
        {
            int nx = x;
            int ny = y;

            if (x < 0) nx = WorldSettings.ChunkSize - 1;
            else if (x >= WorldSettings.ChunkSize) nx = 0;

            if (y < 0) ny = WorldSettings.ChunkSize - 1;
            else if (y >= WorldSettings.ChunkSize) ny = 0;

            return GetBlock(cached.blocks[nx, ny]).id == 0;
        }

        return false;
    }

    private void BlitBlockToChunk(Color[] chunkTexture, Color[] sprite, int blockX, int blockY, int chunkTextureSize)
    {
        for (int i = 0; i < sprite.Length; i++)
        {
            int row = i % WorldSettings.BlockResolution;
            int column = i / WorldSettings.BlockResolution;

            int globalX = blockX * WorldSettings.BlockResolution + row;
            int globalY = blockY * WorldSettings.BlockResolution + column;

            int index = globalX + globalY * chunkTextureSize;

            chunkTexture[index] = sprite[i];
        }
    }

    private void BlitToChunk(Color[] chunkTexture, Color[] sprite, int blockX, int blockY, int chunkTextureSize)
    {
        for (int i = 0; i < sprite.Length; i++)
        {
            int row = i % WorldSettings.BlockResolution;
            int column = i / WorldSettings.BlockResolution;

            int globalX = blockX * WorldSettings.BlockResolution + row;
            int globalY = blockY * WorldSettings.BlockResolution + column;

            int index = globalX + globalY * chunkTextureSize;

            if (sprite[i].a > 0f) // Only draw non-transparent pixels
                chunkTexture[index] = sprite[i];
        }
    }

    public Block GetBlock(byte blockId)
    {
        return blocks[blockId];
    }

    public Dictionary<int, Color[]> edgeOverlays;
    public Color[] GetEdgeOverlay(int mask)
    {
        if (edgeOverlays != null && edgeOverlays.TryGetValue(mask, out var overlay))
            return overlay;
        return null;
    }

    //List<Vector2[]> GeneratePathsFromTexture(Chunk chunk)
    //{
    //    //if (!chunk.IsDirty)
    //    //    return rectanglePath;

    //    List<Vector2[]> paths = new List<Vector2[]>();
    //    byte[,] grid = chunk.blocks;
    //    int chunkSize = WorldSettings.ChunkSize;
    //    float pixelSize = WorldSettings.BlockPhysicalSize;

    //    bool[,] visited = new bool[chunkSize, chunkSize];

    //    for (int y = 0; y < chunkSize; y++)
    //    {
    //        for (int x = 0; x < chunkSize; x++)
    //        {
    //            if (visited[x, y] || !Blocks.data[grid[x, y]].HasCollider)
    //                continue;

    //            // Find the width of the rectangle
    //            int width = 0;
    //            while (x + width < chunkSize && Blocks.data[grid[x + width, y]].HasCollider && !visited[x + width, y])
    //            {
    //                width++;
    //            }

    //            // Find the height by checking if next rows match the current row
    //            int height = 1;
    //            bool validRow = true;
    //            while (y + height < chunkSize && validRow)
    //            {
    //                for (int i = 0; i < width; i++)
    //                {
    //                    if (!Blocks.data[grid[x + i, y + height]].HasCollider || visited[x + i, y + height])
    //                    {
    //                        validRow = false;
    //                        break;
    //                    }
    //                }
    //                if (validRow) height++;
    //            }

    //            // Mark visited cells
    //            for (int i = 0; i < width; i++)
    //                for (int j = 0; j < height; j++)
    //                    visited[x + i, y + j] = true;

    //            // Convert to world coordinates
    //            float xx = (x - chunkSize / 2) * pixelSize;
    //            float yy = (y - chunkSize / 2) * pixelSize;

    //            Vector2[] path = new Vector2[4];
    //            path[0] = new Vector2(xx, yy);
    //            path[1] = new Vector2(xx + width * pixelSize, yy);
    //            path[2] = new Vector2(xx + width * pixelSize, yy + height * pixelSize);
    //            path[3] = new Vector2(xx, yy + height * pixelSize);

    //            paths.Add(path);
    //        }
    //    }

    //    return paths;
    //}

    List<Vector2[]> GeneratePathsFromTexture(Chunk chunk)
    {
        List<Vector2[]> paths = new List<Vector2[]>();
        byte[,] grid = chunk.blocks;
        int chunkSize = WorldSettings.ChunkSize;
        float pixelSize = WorldSettings.BlockPhysicalSize;

        for (int x = 0; x < chunkSize; x++)
        {
            int startY = -1;

            for (int y = 0; y < chunkSize; y++)
            {
                if (!Blocks.data[grid[x, y]].HasCollider)
                {
                    if (startY != -1)
                    {
                        float xx = (x - chunkSize / 2f) * pixelSize;
                        float yy = (startY - chunkSize / 2f) * pixelSize;

                        Vector2[] path = new Vector2[4];
                        path[0] = new Vector2(xx, yy);
                        path[1] = new Vector2(xx + pixelSize, yy);
                        path[2] = new Vector2(xx + pixelSize, yy + (y - startY) * pixelSize);
                        path[3] = new Vector2(xx, yy + (y - startY) * pixelSize);

                        paths.Add(path);
                        startY = -1;
                    }
                }
                else
                {
                    if (startY == -1) startY = y;
                }
            }

            if (startY != -1)
            {
                float xx = (x - chunkSize / 2f) * pixelSize;
                float yy = (startY - chunkSize / 2f) * pixelSize;

                Vector2[] path = new Vector2[4];
                path[0] = new Vector2(xx, yy);
                path[1] = new Vector2(xx + pixelSize, yy);
                path[2] = new Vector2(xx + pixelSize, yy + (chunkSize - startY) * pixelSize);
                path[3] = new Vector2(xx, yy + (chunkSize - startY) * pixelSize);   

                paths.Add(path);
            }
        }

        return paths;
    }
}
