using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldActivator : MonoBehaviour
{

    Transform player;

    public int verticalActiveRange = 4;
    public int horizontalActiveRange = 6;

    List<Chunk> activeChunks = new List<Chunk>();
    [SerializeField] List<Chunk> toBeActivatedChunks = new List<Chunk>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2Int playerChunkPos = ChunkHelper.PhysicalToChunk(player.transform.position);

        if (PlayerMoved(playerChunkPos))
        {
            CheckChunks(playerChunkPos);
        }
    }

    private void FixedUpdate()
    {
        Vector2Int playerChunkPos = ChunkHelper.PhysicalToChunk(player.transform.position);

        if (PlayerMoved(playerChunkPos))
        {
            CheckChunks(playerChunkPos);
        }
        DeactivateOutOfRangeChunks(playerChunkPos);

        if (toBeActivatedChunks.Count > 0)
            StartCoroutine(ActivateChunks());

    }

    void DeactivateOutOfRangeChunks(Vector2Int playerChunkPos)
    {
        for (int i = 0; i < activeChunks.Count; i++)
        {
            Chunk chunk = activeChunks[i];

            int absX = Mathf.Abs(playerChunkPos.x - chunk.position.x);
            int absY = Mathf.Abs(playerChunkPos.y - chunk.position.y);

            if (absX > horizontalActiveRange / 2 + 1 || absY > verticalActiveRange / 2 + 1)
            {
                WorldManager.Instance.chunkPooler.ReturnChunk(chunk.chunkWorld);
                activeChunks.Remove(chunk);

                if (toBeActivatedChunks.Contains(chunk))
                    toBeActivatedChunks.Remove(chunk);
            }
        }
    }

    Vector2Int oldChunkPos = new Vector2Int(-55, 55);
    bool PlayerMoved(Vector2Int playerChunkPos)
    {
        if (!playerChunkPos.Equals(oldChunkPos))
        {
            oldChunkPos = playerChunkPos;
            return true;
        }

        return false;
    }

    void CheckChunks(Vector2Int playerChunkPos)
    {
        int startX = -horizontalActiveRange / 2;
        int startY = -verticalActiveRange / 2;

        for (int x = startX; x < startX + horizontalActiveRange; x++)
        {
            int chunkX = x + playerChunkPos.x;
            if (chunkX < 0 || chunkX >= WorldSettings.WorldSizeX)
                continue;
            for (int y = startY; y < startY + verticalActiveRange; y++)
            {
                int chunkY = y + playerChunkPos.y;
                if (chunkY < 0 || chunkY >= WorldSettings.WorldSizeY)
                    continue;

                Chunk chunk = WorldManager.Instance.GetChunk(new Vector2Int(chunkX, chunkY));

                if (!activeChunks.Contains(chunk) && !toBeActivatedChunks.Contains(chunk))
                    toBeActivatedChunks.Add(chunk);
            }
        }
    }

    IEnumerator ActivateChunks()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < toBeActivatedChunks.Count; i++)
        {
            Chunk chunk = toBeActivatedChunks[i];

            WorldManager.Instance.SpawnChunk(chunk);

            toBeActivatedChunks.RemoveAt(i);
            activeChunks.Add(chunk);
            i--;

            if (stopwatch.ElapsedMilliseconds > 1f / 60)
            {
                yield return null;
                stopwatch.Reset();
                stopwatch.Start();
            }
        }
    }
}
