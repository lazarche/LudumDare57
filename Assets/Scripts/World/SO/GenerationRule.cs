using UnityEngine;

[CreateAssetMenu(fileName = "GenerationRule", menuName = "Miner/GenerationRule")]
public class GenerationRule : ScriptableObject
{
    public byte blockID;
    public int startHeight;
    public AnimationCurve transitionCurve;
}
