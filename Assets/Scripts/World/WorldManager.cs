using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Singleton
    static WorldManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance);
        }

    }

    public static WorldManager Instance { get { return instance; } }
    #endregion

    [SerializeField] WorldBuilder worldBuilder;
    [SerializeField] WorldGenerator worldGenerator;
    public WorldActivator worldActivator;
    public ChunkPooler chunkPooler;

    Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();

    void Start()
    {
        //for (int x = 0; x < WorldSettings.WorldSizeX; x++)
        //{
        //    for (int y = 0; y < WorldSettings.WorldSizeY; y++)
        //    {
        //        Vector2Int chunkPos = new Vector2Int(x, y);
        //        chunks.Add(chunkPos, new Chunk { position = chunkPos });
        //    }
        //}
    }

    public void SpawnChunk(Chunk chunk)
    {
        if(!chunk.IsGenerated)
            worldGenerator.GenerateChunk(chunk);

        worldBuilder.BuildChunk(chunk);
        ChunkWorld chunkWorld = chunkPooler.GetChunk();
        chunkWorld.Init(chunk);
        chunkWorld.gameObject.SetActive(true);
    }

    public void UpdateChunk(Chunk chunk)
    {
        worldBuilder.BuildChunk(chunk);
        ChunkWorld chunkWorld = chunkPooler.GetChunk();
        chunkWorld.UpdateChunk(chunk);
    }

    internal Chunk GetChunk(Vector2Int vector2Int)
    {
        if(!chunks.ContainsKey(vector2Int))
        {
            Vector2Int chunkPos = new Vector2Int(vector2Int.x, vector2Int.y);
            chunks.Add(chunkPos, new Chunk { position = chunkPos });
        }
        return chunks[vector2Int];
    }
}
