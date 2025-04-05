using System;
using UnityEngine;

public class PlayerMiner : MonoBehaviour
{
    public LayerMask groundLayer;
    Vector2 input = Vector2.zero;
    
    float mineCouldownTime = 0.5f;
    float mineCooldown = 0f;
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Input.GetKey(KeyCode.K) && mineCooldown <= 0f)
        {
            Mine();
            mineCooldown = mineCouldownTime;
        }
        else
        {
            mineCooldown -= Time.deltaTime;
        }
    }

    private void Mine()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, input, 3f, groundLayer);
        if (hit.collider != null && hit.transform.gameObject.GetComponent<ChunkWorld>())
        {
            Chunk chunk = hit.transform.gameObject.GetComponent<ChunkWorld>().chunk;
            Vector2 hitPos = hit.point -  ((Vector2)transform.position - hit.point).normalized * 0.02f;
            WorldManager.Instance.worldMiner.Mine(chunk, hitPos);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)input);
    }
}
