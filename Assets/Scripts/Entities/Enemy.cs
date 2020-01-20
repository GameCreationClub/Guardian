using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private Animator animator;

    public GameObject moveToIndicator;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        moveToIndicator = GameManager.instance.enemyMoveTo;
    }

    protected void MovementAi(Entity currentTarget, Vector2 currentTargetPositionAim)
    {
        if ((currentTargetPositionAim - Vector2Position).normalized.Equals(facingDirection))
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

    public override void Move(Vector2 direction)
    {
        StartCoroutine(MoveCoroutine(direction));
    }

    private IEnumerator MoveCoroutine(Vector2 direction)
    {
        Vector2 moveTo = Vector2Position + direction;
        moveToIndicator.transform.position = moveTo;
        moveToIndicator.SetActive(true);
        animator.Play("Enemy_Fade_Out");

        yield return new WaitForSeconds(1f);

        moveToIndicator.SetActive(false);
        animator.Play("Enemy_Fade_In");
        MoveTo(moveTo);
    }
}
