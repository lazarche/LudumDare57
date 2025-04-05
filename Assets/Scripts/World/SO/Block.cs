using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Miner/Block")]
public class Block : ScriptableObject
{
    public byte id;
    public string blockName;
    public Sprite[] sprites;

    List<Color[]> colors;

    public void LoadColors()
    {
        colors = new List<Color[]>();
        foreach (Sprite sprite in sprites)
        {
            Texture2D texture = sprite.texture;
            Color[] pixels = texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height);
            colors.Add(pixels);
        }
    }

    public Color[] GetSprite()
    {
        return colors[Random.Range(0, sprites.Length)];
    }
}
