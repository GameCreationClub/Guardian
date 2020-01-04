using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected void MovementAi(Entity currentTarget, Vector2 currentTargetPositionAim)
    {
        if ((currentTargetPositionAim - Vector2Position).normalized.Equals(facingDirection))
        {
            if (Vector2.Distance(Vector2Position, currentTargetPositionAim) > 1.42f)
            {
                Move(facingDirection * init);
            }
            else if (Vector2.Distance(Vector2Position, currentTargetPositionAim) >= 1f)
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
}
