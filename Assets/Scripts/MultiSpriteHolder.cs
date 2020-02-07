using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteHolder : MonoBehaviour
{

    public Sprite[] sprites;
    private SpriteRenderer sprite_renderer;

    public Sprite[] alternatives;

    private int last_index = 0;

    public void SetSprite(int index, bool alternative = false)
    {
        if (sprite_renderer == null) sprite_renderer = GetComponent<SpriteRenderer>();
        if (index < sprites.Length && index >= 0)
        {
            if (alternative && index < alternatives.Length && alternatives[index] != null)
                sprite_renderer.sprite = alternatives[index];
            else
                sprite_renderer.sprite = sprites[index];
            last_index = index;
        }
    }

    public void makeAlt()
    {
        SetSprite(last_index);
    }

    public int length()
    {
        return sprites.Length;
    }

}
