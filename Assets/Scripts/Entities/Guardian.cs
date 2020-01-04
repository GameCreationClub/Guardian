using UnityEngine;

public class Guardian : Entity
{
    public override void MovementTurn()
    {
        print("Guardian movement");

    }

    public override void AttackTurn()
    {
        base.AttackTurn();
        print("Guardian attack");
    }
}
