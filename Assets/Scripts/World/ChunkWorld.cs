using UnityEngine;

public class ChunkWorld : MonoBehaviour
{
    public Chunk chunk;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PolygonCollider2D polygonCollider;

    public void Init(Chunk chunk)
    {
        this.chunk = chunk;
        this.chunk.chunkWorld = this;

        transform.position = new Vector3(chunk.position.x * WorldSettings.ChunkPhysicalSize + WorldSettings.ChunkPhysicalSize / 2, -chunk.position.y * WorldSettings.ChunkPhysicalSize - WorldSettings.ChunkPhysicalSize / 2, 0);
       
        Texture2D texture = new Texture2D(WorldSettings.ChunkSize * WorldSettings.BlockResolution, WorldSettings.ChunkSize * WorldSettings.BlockResolution);
        texture.SetPixels(0, 0, WorldSettings.ChunkSize * WorldSettings.BlockResolution, WorldSettings.ChunkSize * WorldSettings.BlockResolution, chunk.texture);
        texture.filterMode = FilterMode.Point;
        //texture.wrapMode = TextureWrapMode.Clamp;

        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), WorldSettings.BlockResolution);
        spriteRenderer.sprite = sprite;

        //Collider
        polygonCollider.enabled = false;
        polygonCollider.pathCount = chunk.collider.Count;
        for (int i = 0; i < chunk.collider.Count; i++)
            polygonCollider.SetPath(i, chunk.collider[i]);
        polygonCollider.enabled = true;

        //Name
        gameObject.name = "Chunk: " + chunk.position.x + "," + chunk.position.y;
    }

    public void UpdateChunk()
    {
        Texture2D texture = new Texture2D(WorldSettings.ChunkSize * WorldSettings.BlockResolution, WorldSettings.ChunkSize * WorldSettings.BlockResolution);
        texture.SetPixels(0, 0, WorldSettings.ChunkSize * WorldSettings.BlockResolution, WorldSettings.ChunkSize * WorldSettings.BlockResolution, chunk.texture);
        texture.filterMode = FilterMode.Point;
        //texture.wrapMode = TextureWrapMode.Clamp;

        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), WorldSettings.BlockResolution);
        spriteRenderer.sprite = sprite;

        //Collider
        polygonCollider.enabled = false;
        polygonCollider.pathCount = chunk.collider.Count;
        for (int i = 0; i < chunk.collider.Count; i++)
            polygonCollider.SetPath(i, chunk.collider[i]);
        polygonCollider.enabled = true;
    }
}
