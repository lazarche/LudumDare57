using UnityEngine;

public class WorldSettings
{
    public static readonly int ChunkSize = 8;
    public static float ChunkPhysicalSize { get { return ChunkSize * BlockPhysicalSize; } }

    public static readonly int WorldSizeX = 64;
    public static readonly int WorldSizeY = 2048;

    public static readonly int BlockResolution = 8;
    public static readonly int BlockPhysicalSize = 1;

}
