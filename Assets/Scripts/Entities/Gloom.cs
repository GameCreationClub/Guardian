using UnityEngine;

public class Gloom : Entity
{
    private Entity[] players;
    private Entity currentTarget;

    private void Start()
    {
        players = GameManager.instance.players.ToArray();
    }

    public override void MovementTurn()
    {
        print("Gloom movement");

        currentTarget = players[0];
        for (int i = 1; i < players.Length; i++)
        {
            if (Vector2.Distance(Vector2Position, players[i].Vector2Position) < Vector2.Distance(Vector2Position, currentTarget.Vector2Position))
                currentTarget = players[i];
        }

        Vector2 currentTargetPosition = currentTarget.Vector2Position - GameManager.RoundVector2((currentTarget.Vector2Position - Vector2Position).normalized);

        if (CanAttack(currentTarget.Vector2Position))
        {
            GameManager.instance.NextTurn();
            return;
        }

        if ((currentTargetPosition - Vector2Position).normalized.Equals(facingDirection))
        {
            if (Vector2.Distance(Vector2Position, currentTargetPosition) > 1.42f)
            {
                Move(facingDirection * init);
            }
            else if (Vector2.Distance(Vector2Position, currentTargetPosition) >= 1f)
            {
                Move(facingDirection);
            }
            else
            {
                GameManager.instance.NextTurn();
            }
        }
        else
        {
            Vector2 normalizedDistance = (currentTarget.Vector2Position - Vector2Position).normalized;
            Vector2 absNormalizedDistance = GameManager.AbsVector2(normalizedDistance);
            Vector2 roundNormalizedDistance = GameManager.RoundVector2(normalizedDistance);

            if (absNormalizedDistance.Equals(Vector2.right) || absNormalizedDistance.Equals(Vector2.up))
            {
                RotateTo(roundNormalizedDistance);
            }
            else
            {
                if (facingDirection.Equals(roundNormalizedDistance))
                {
                    Move(GameManager.ExtremeCeilVector2(normalizedDistance));
                }
                else
                {
                    RotateTo(roundNormalizedDistance);
                }
            }
        }
    }

    public override void AttackTurn()
    {
        print("Gloom attack");

        if (CanAttack(currentTarget.Vector2Position))
            Attack(currentTarget);
        else
            GameManager.instance.NextTurn();
    }
}
