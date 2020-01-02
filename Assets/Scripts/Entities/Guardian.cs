using UnityEngine;

public class Guardian : Entity
{
    public override void MovementTurn()
    {
        print("Guardian movement");
        MoveTo(new Vector2(8, 6));
    }

    public override void AttackTurn()
    {
        print("Guardian attack");
        Attack(FindObjectOfType<Aspect>());
        gameManager.NextTurn();
    }
}
