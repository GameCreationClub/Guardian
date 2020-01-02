using System.Collections;
using UnityEngine;

public abstract class Entity : Object
{
    public int hp, atk, init;
    public Vector2 facingDirection;

    protected int maxHp;

    private Vector3 moveTo;
    private float moveIncrement;

    private void Awake()
    {
        maxHp = hp;

        moveIncrement = init * Time.fixedDeltaTime;

        transform.Translate(Vector3.back);
    }

    public abstract void MovementTurn();
    public abstract void AttackTurn();

    public void Move(Vector2 direction)
    {
        MoveTo(Vector2Position + direction);
    }

    public void MoveTo(Vector2 position)
    {
        X = (int)position.x;
        Y = (int)position.y;
        moveTo = Vector2Position;

        StopCoroutine(MovementAnimation());
        StartCoroutine(MovementAnimation());
    }

    public void RotateTo(Vector2 rotation)
    {
        facingDirection = rotation;
    }

    public void Attack(Entity e)
    {
        if (e != null)
            e.TakeDamage(atk);
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
        GameManager.instance.RemoveEntity(this);

        Destroy(gameObject);
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
