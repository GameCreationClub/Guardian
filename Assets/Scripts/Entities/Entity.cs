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
}
