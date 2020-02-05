using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteHolder : MonoBehaviour
{

    public Sprite[] sprites;
    private SpriteRenderer sprite_renderer;

    public void SetSprite(int index)
    {
        if (sprite_renderer == null) sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = sprites[0];
        if (index < sprites.Length && index >= 0)
            sprite_renderer.sprite = sprites[index];
    }

    public int length()
    {
        return sprites.Length;
    }

}
