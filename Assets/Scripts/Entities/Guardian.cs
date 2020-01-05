using UnityEngine;

public class Guardian : Entity
{
    public override void AttackTurn()
    {
        base.AttackTurn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit"))
            GameManager.instance.PlayerExit();
    }
}
