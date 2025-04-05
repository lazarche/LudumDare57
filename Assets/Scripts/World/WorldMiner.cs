using UnityEngine;

public class WorldMiner : MonoBehaviour
{
    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Mine(mousePos);
    //    }
    //}

    public void Mine(Chunk chunk, Vector2 position)
    {
        Vector2Int blockPos = ChunkHelper.PhysicalToBlock(position);

        Debug.Log(blockPos);
        chunk.blocks[blockPos.x, blockPos.y] = 0;
        
        WorldManager.Instance.UpdateChunk(chunk);
        CheckSurrounding(blockPos, chunk.position);
    }

    void CheckSurrounding(Vector2Int blockPos, Vector2Int chunkPos)
    {
        Chunk toCheckX = null;
        if(blockPos.x == 0)
            toCheckX = WorldManager.Instance.GetChunk(new Vector2Int(chunkPos.x - 1, chunkPos.y));
        else if (blockPos.x == WorldSettings.ChunkSize - 1)
            toCheckX = WorldManager.Instance.GetChunk(new Vector2Int(chunkPos.x + 1, chunkPos.y));

        Chunk toCheckY = null;
        if (blockPos.y == 0)
            toCheckY = WorldManager.Instance.GetChunk(new Vector2Int(chunkPos.x, chunkPos.y + 1));
        else if (blockPos.y == WorldSettings.ChunkSize - 1)
            toCheckY = WorldManager.Instance.GetChunk(new Vector2Int(chunkPos.x, chunkPos.y - 1));
        
        if (toCheckX != null)
            if (toCheckX.IsGenerated)
                WorldManager.Instance.UpdateChunk(toCheckX);

        if (toCheckY != null)
            if (toCheckY.IsGenerated)
                WorldManager.Instance.UpdateChunk(toCheckY);
    }
}
