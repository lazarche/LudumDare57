using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Miner/Block")]
public class Block : ScriptableObject
{
    public byte id;
    public string blockName;
    public Sprite[] sprites;
    public bool HasCollider = true;

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

    public Color[] GetSprite(float perlin)
    {
        int index = Mathf.FloorToInt(perlin * (sprites.Length - 1));
        return colors[index];
    }

    public override bool Equals(object other)
    {
        return this.id == ((Block)other).id;
    }
}
