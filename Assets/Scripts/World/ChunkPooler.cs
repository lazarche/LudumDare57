using System.Collections.Generic;
using UnityEngine;

public class ChunkPooler : MonoBehaviour
{
    [SerializeField] Transform world;
    Stack<ChunkWorld> pooledChunks;
    [SerializeField] GameObject chunkPrefab;
    private void Start()
    {
        pooledChunks = new Stack<ChunkWorld>();
        PreInstantiate();
    }

    void PreInstantiate()
    {
        int neededChunks = (WorldManager.Instance.worldActivator.verticalActiveRange + 1) * (WorldManager.Instance.worldActivator.horizontalActiveRange + 1) * 2;
        for (int i = 0; i < neededChunks; i++)
        {
            GameObject chunk = Instantiate(chunkPrefab);
            chunk.transform.SetParent(world);
            chunk.SetActive(false);
            pooledChunks.Push(chunk.GetComponent<ChunkWorld>());
        }
    }

    public ChunkWorld GetChunk()
    {
        return pooledChunks.Pop();
    }

    public void ReturnChunk(ChunkWorld world)
    {
        world.gameObject.SetActive(false);
        pooledChunks.Push(world);
    }
}
