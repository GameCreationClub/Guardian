using UnityEngine;

public class Gloom : Enemy
{
    private Entity[] players;
    private Entity currentTarget;

    private void Start()
    {
        players = GameManager.instance.players.ToArray();
    }

    public override void MovementTurn()
    {
        currentTarget = players[0];
        for (int i = 1; i < players.Length; i++)
        {
            if (Vector2.Distance(Vector2Position, players[i].Vector2Position) < Vector2.Distance(Vector2Position, currentTarget.Vector2Position) || currentTarget.isDead)
                currentTarget = players[i];
        }

        Vector2 currentTargetPosition = currentTarget.Vector2Position - GameManager.RoundVector2((currentTarget.Vector2Position - Vector2Position).normalized);

        if (CanAttack(currentTarget.Vector2Position))
            GameManager.instance.NextTurn();
        else
            MovementAi(currentTarget, currentTargetPosition);
    }

    public override void AttackTurn()
    {
        if (CanAttack(currentTarget.Vector2Position))
            Attack(currentTarget);
        else
            GameManager.instance.NextTurn();
    }
}
