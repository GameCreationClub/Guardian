using System.Collections;
using UnityEngine;

public abstract class Entity : Object
{
    public int hp, atk, init;
    public bool canRotate180, isDead = false;
    public Vector2 facingDirection = Vector2.up;

    protected int maxHp;
    private bool isPlayer;

    private Vector3 moveTo;
    private float moveIncrement;

    private void Awake()
    {
        maxHp = hp;
        isPlayer = CompareTag("Player");

        moveIncrement = init * Time.deltaTime;

        transform.Translate(Vector3.back);
        facingDirection = Vector2.up;
    }

    public virtual void MovementTurn()
    {
    }

    public virtual void AttackTurn()
    {
        Entity[] entities = GameManager.instance.entities.ToArray();

        foreach (Entity e in entities)
        {
            if ((isPlayer && e.CompareTag("Player")) || (!isPlayer && !e.CompareTag("Player")))
                continue;

            else
            {
                if (CanAttack(e.Vector2Position))
                    return;
            }
        }

        GameManager.instance.NextTurn();
        return;
    }

    public void Move(Vector2 direction)
    {
        MoveTo(Vector2Position + direction);
    }

    public void KnockBack(Vector2 direction)
    {
        Vector2Position += direction;
        transform.position += (Vector3)direction;
    }

    public void MoveTo(Vector2 position)
    {
        X = (int)position.x;
        Y = (int)position.y;
        moveTo = Vector2Position;

        StopCoroutine(MovementAnimation());
        StartCoroutine(MovementAnimation());
    }

    public bool CanMoveTo(Vector2 moveToPos)
    {
        if (GameManager.instance.IsEntityAtPosition(moveToPos))
            return false;

        else
        {
            if ((moveToPos - Vector2Position).normalized.Equals(facingDirection))
            {
                return Vector2.Distance(moveToPos, Vector2Position) <= init;
            }
            else
            {
                Vector2 moveInFacingDirection = Vector2Position + facingDirection;
                Vector2 distanceFromMove = moveToPos - moveInFacingDirection;

                return GameManager.AbsVector2(GameManager.FlipVector2(distanceFromMove)).Equals(GameManager.AbsVector2(facingDirection));
            }
        }
    }

    public void RotateTo(Vector2 rotation)
    {
        facingDirection = rotation;
        GameManager.instance.NextTurn();
    }

    public bool CanRotateTo(Vector2 rotateTo)
    {
        Vector2 absRotateTo = GameManager.AbsVector2(rotateTo);

        if (rotateTo.Equals(facingDirection))
        {
            return false;
        }
        else if (canRotate180)
        {
            return absRotateTo.x != absRotateTo.y && (absRotateTo.x == 1f || absRotateTo.y == 1f);
        }
        else
        {
            return absRotateTo.x != Mathf.Abs(facingDirection.x) && absRotateTo.y != Mathf.Abs(facingDirection.y) && (absRotateTo.x == 1f || absRotateTo.y == 1f);
        }
    }

    public void Attack(Entity e)
    {
        if (e != null && e != this)
        {
            print(name + " attacked " + e.name + " for " + atk + " damage");
            GameManager.instance.NextTurn();
            e.TakeDamage(atk);
        }
    }

    public bool CanAttack(Vector2 attack)
    {
        Vector2 moveInFacingDirection = Vector2Position + facingDirection;
        Vector2 distanceFromMove = attack - moveInFacingDirection;

        return distanceFromMove.Equals(Vector2.zero) || GameManager.AbsVector2(GameManager.FlipVector2(distanceFromMove)).Equals(GameManager.AbsVector2(facingDirection));
    }

    protected void TakeDamage(int damage)
    {
        ChangeHp(-damage);
    }

    protected void TakeHealing(int healing)
    {
        ChangeHp(healing);
    }

    protected void Die()
    {
        print(name + " died");
        isDead = true;
        gameObject.SetActive(false);
    }

    protected void ChangeHp(int change)
    {
        hp += change;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
        else if (hp > maxHp)
        {
            hp = maxHp;
        }
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
            GameManager.instance.NextTurn();
        }
    }
}
