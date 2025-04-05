using UnityEngine;

[CreateAssetMenu(fileName = "GenerationCaveRule", menuName = "Miner/GenerationCaveRule")]
public class GenerationCaveRule : ScriptableObject
{
    public int startHeight;
    public float scale;
    public float threshold;
}
