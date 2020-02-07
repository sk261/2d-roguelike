using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Script : MovingObject
{

    public int damageReactionThreshold = 5;

    public int wallDamage = 1;

    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;

    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;
    public Text foodText;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food: " + food;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    private void Update()
    {
        if (!GameManager.instance.playersTurn) return;
        int dir_x = 0, dir_y = 0;

        dir_x = (int)(Input.GetAxisRaw("Horizontal"));
        dir_y = (int)(Input.GetAxisRaw("Vertical"));

        if (dir_x != 0) dir_y = 0;

        if (dir_x + dir_y != 0) AttemptMove<Wall>(dir_x, dir_y);
    }

    protected override void AttemptMove<T>(int xMove, int yMove)
    {
        AlterFood(-1);
        base.AttemptMove<T>(xMove, yMove);
        RaycastHit2D hit;
        if (Move(xMove,yMove, out hit))
        {

        }
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.Strike(wallDamage);
        animator.SetTrigger("playerChop");
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food")
        {
            AlterFood(pointsPerFood);
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            AlterFood(pointsPerSoda);
            collision.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void AlterFood(int value)
    {
        food += value;
        foodText.text = value + " Food: " + food;
        if (value < 0)
        {
            if (damageReactionThreshold < -value || food < damageReactionThreshold)
                animator.SetTrigger("playerHit");
            CheckIfGameOver();
        }
    }

    private void CheckIfGameOver()
    {
        if (food <= 0) GameManager.instance.GameOver();
    }

}