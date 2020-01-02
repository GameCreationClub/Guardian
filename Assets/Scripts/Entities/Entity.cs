using UnityEngine;

public abstract class Entity : Object
{
    public int hp, atk, init;
    public Vector2 facingDirection;

    public abstract void MovementTurn();
    public abstract void AttackTurn();
}
