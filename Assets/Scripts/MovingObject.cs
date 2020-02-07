using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveInterval = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D entityCollider;
    private Rigidbody2D entityRB;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        entityCollider = GetComponent<BoxCollider2D>();
        entityRB = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveInterval;
    }

    protected bool Move(int xMove, int yMove, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xMove, yMove);

        entityCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        entityCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMove(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMove(Vector3 end)
    {

        float dist_sqr = (transform.position - end).sqrMagnitude;

        while (dist_sqr > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(entityRB.position, end, inverseMoveTime * Time.deltaTime);
            entityRB.MovePosition(newPos);
            dist_sqr = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xMove, int yMove)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xMove, yMove, out hit);
        if (hit.transform == null)
            return;
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}