
using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public AudioClip[] chopSounds;
    public int hp = 3;

    private MultiSpriteHolder wall;

    void Awake()
    {
        wall = GetComponent<MultiSpriteHolder>();
    }

    public void Strike(int dmg)
    {
   //    SoundManager.instance.PlaySingle(chopSounds[Random.Range(0, chopSounds.Length)]);
        wall.makeAlt();
        hp -= dmg;
        if (hp <= 0) gameObject.SetActive(false);
    }
}
