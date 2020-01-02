using System.Collections;
using UnityEngine;

public abstract class Entity : Object
{
    public int hp, atk, init;
    public Vector2 facingDirection;

    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    /*private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, new Vector2(X, Y)) > 0.025f)
        {
            transform.Translate((new Vector3(X, Y) - transform.position).normalized * init * Time.fixedDeltaTime);
        }
    }*/

    public abstract void MovementTurn();
    public abstract void AttackTurn();

    public void Move(Vector2 direction)
    {
        MoveTo(new Vector2(X + direction.x, Y + direction.y));
    }

    public void MoveTo(Vector2 position)
    {
        X = (int)position.x;
        Y = (int)position.y;

        StartCoroutine(MovementAnimation());
    }

    protected IEnumerator MovementAnimation()
    {
        yield return new WaitForEndOfFrame();

        if (Vector2.Distance(transform.position, new Vector2(X, Y)) > 0.025f)
        {
            transform.Translate((new Vector3(X, Y) - transform.position).normalized * init * Time.fixedDeltaTime);
            StartCoroutine(MovementAnimation());
        }
        else
        {
            StopCoroutine(MovementAnimation());
            gameManager.NextTurn();
        }
    }
}
