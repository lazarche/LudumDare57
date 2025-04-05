using UnityEngine;

public class Blocks : MonoBehaviour
{
    public static Block[] data;
    void Awake()
    {
        data = Resources.LoadAll<Block>("Blocks");
    }
}
