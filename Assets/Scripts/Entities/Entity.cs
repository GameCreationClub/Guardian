using System.Collections;
using UnityEngine;

public abstract class Entity : Object
{
    public int hp, atk, init;
    public Vector2 facingDirection;

    protected GameManager gameManager;

    private Vector3 moveTo;
    private float moveIncrement;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        moveIncrement = init * Time.fixedDeltaTime;
    }

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
        moveTo = new Vector2(X, Y);

        StartCoroutine(MovementAnimation());
    }

    protected IEnumerator MovementAnimation()
    {
        yield return new WaitForEndOfFrame();

        if (Vector2.Distance(transform.position, moveTo) > moveIncrement)
        {
            transform.Translate((moveTo - transform.position).normalized * moveIncrement);
            StartCoroutine(MovementAnimation());
        }
        else
        {
            transform.position = moveTo;

            StopCoroutine(MovementAnimation());
            gameManager.NextTurn();
        }
    }
}
