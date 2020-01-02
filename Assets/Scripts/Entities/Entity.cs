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

        transform.position = new Vector2(X, Y);
    }
}
