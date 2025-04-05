using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    float scale = 20f;
    public GenerationRule[] blockRules;
    public GenerationCaveRule[] caveRules;
    private void Awake()
    {
        blockRules = Resources.LoadAll<GenerationRule>("GenerationRules/Blocks");
        System.Array.Sort(blockRules, (a, b) => a.startHeight.CompareTo(b.startHeight));

        caveRules = Resources.LoadAll<GenerationCaveRule>("GenerationRules/Caves");
        System.Array.Sort(caveRules, (a, b) => a.startHeight.CompareTo(b.startHeight));
    }

    public void GenerateChunk(Chunk chunk)
    {
        for (int x = 0; x < WorldSettings.ChunkSize; x++)
        {
            for (int y = 0; y < WorldSettings.ChunkSize; y++)
            {
                int globalX = x + chunk.position.x * WorldSettings.ChunkSize;
                int globalY = (WorldSettings.ChunkSize - 1 - y) + (chunk.position.y * WorldSettings.ChunkSize);

                if(CheckForCave(globalX, globalY))
                {
                    chunk.blocks[x, y] = 0;
                    continue;
                }

                float scaledX = globalX / scale;
                float scaledY = globalY / scale;
                float perlinValue = Mathf.PerlinNoise(scaledX, scaledY);
                chunk.blocks[x, y] = GetBlockType(globalY, perlinValue);
            }
        }

        chunk.IsGenerated = true;
    }

    byte GetBlockType(int y, float perlinValue)
    {
        for (int i = 0; i < blockRules.Length - 1; i++)
        {
            GenerationRule currentRule = blockRules[i];
            GenerationRule nextRule = blockRules[i + 1];

            if (y >= currentRule.startHeight && y < nextRule.startHeight)
                return GetInterpolatedBlock(currentRule.blockID, nextRule.blockID, y, currentRule.startHeight, nextRule.startHeight, perlinValue, currentRule.transitionCurve);
        }

        return blockRules[blockRules.Length - 1].blockID;

    }

    byte GetInterpolatedBlock(byte primary, byte secondary, int y, int startHeight, int endHeight, float perlinValue, AnimationCurve curve)
    {
        float progress = Mathf.InverseLerp(startHeight, endHeight, y);
        float threshold = curve.Evaluate(progress);
        return perlinValue < threshold ? primary : secondary;
    }

    bool CheckForCave(int x, int y)
    {
        GenerationCaveRule generationCaveRule = null;
        for (int i = 0; i < caveRules.Length-1; i++)
        {
            GenerationCaveRule currentRule = caveRules[i];
            GenerationCaveRule nextRule = caveRules[i + 1];
            if (y >= currentRule.startHeight && y < nextRule.startHeight)
            {
                generationCaveRule = currentRule;
                break;
            }
            generationCaveRule = nextRule;
        }

        float noise = Mathf.PerlinNoise(x / generationCaveRule.scale, y / generationCaveRule.scale);
        return noise < generationCaveRule.threshold;

    }

}
