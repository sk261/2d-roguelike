using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MovingObject
{
    public int playerDamage;
    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xMove, int yMove)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xMove, yMove);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xMove = 0, yMove = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yMove = target.position.y > transform.position.y ? 1 : -1;
        else
            xMove = target.position.x > transform.position.x ? 1 : -1;
        AttemptMove<Player_Script>(xMove, yMove);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player_Script hitPlayer = component as Player_Script;
        hitPlayer.AlterFood(-playerDamage);
        animator.SetTrigger("enemyAttack");
    }
}