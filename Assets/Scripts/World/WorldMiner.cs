using UnityEngine;

public class WorldMiner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Mine(mousePos);
        }
    }

    void Mine(Vector2 position)
    {
        Chunk chunk = WorldManager.Instance.GetChunk(ChunkHelper.PhysicalToChunk(position));
        Vector2Int blockPos = ChunkHelper.PhysicalToBlock(position);

        Debug.Log(chunk.position + " " + blockPos);

        chunk.blocks[blockPos.x, blockPos.y] = 0;
        
        WorldManager.Instance.UpdateChunk(chunk);
    }
}
