using UnityEngine;

public class Prowler : Enemy
{
    private Entity currentTarget, adventurer, guardian;

    private void Start()
    {
        adventurer = FindObjectOfType<Adventurer>();
        guardian = FindObjectOfType<Guardian>();
    }

    private void MovementAi(Vector2 currentTargetPositionAim)
    {
        if (Vector2Position.Equals(currentTargetPositionAim))
        {
            RotateTo((currentTarget.Vector2Position - Vector2Position).normalized);
        }
        else if ((currentTargetPositionAim - Vector2Position).normalized.Equals(facingDirection))
        {
            if (Vector2.Distance(Vector2Position, currentTargetPositionAim) >= 1.42f && CanMoveTo(Vector2Position + facingDirection * init))
            {
                Move(facingDirection * init);
            }
            else if (Vector2.Distance(Vector2Position, currentTargetPositionAim) >= 1f && CanMoveTo(Vector2Position + facingDirection))
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
            Vector2 normalizedDistance = (currentTargetPositionAim - Vector2Position).normalized;
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

    public override void MovementTurn()
    {
        if (adventurer == null || adventurer.isDead)
            currentTarget = guardian;
        else
            currentTarget = adventurer;

        if ((currentTarget.Vector2Position - Vector2Position).Equals(facingDirection))
            GameManager.instance.NextTurn();

        else
        {
            Vector2[] possibleTargets = { currentTarget.facingDirection * -1, GameManager.FlipVector2(currentTarget.facingDirection), GameManager.FlipVector2(currentTarget.facingDirection) * -1 };
            Vector2 currentTargetPosition = possibleTargets[0];

            for (int i = 1; i < possibleTargets.Length; i++)
            {
                if (Vector2.Distance(Vector2Position, possibleTargets[i]) < Vector2.Distance(Vector2Position, currentTargetPosition))
                    currentTargetPosition = possibleTargets[i];
            }

            currentTargetPosition += currentTarget.Vector2Position;
            MovementAi(currentTargetPosition);
        }
    }

    public override void AttackTurn()
    {
        if (CanAttack(currentTarget.Vector2Position))
        {
            if (currentTarget.CanAttack(Vector2Position))
                atk = 2;
            else
                atk = 4;

            Attack(currentTarget);
        }
        else
        {
            GameManager.instance.NextTurn();
        }
    }
}
