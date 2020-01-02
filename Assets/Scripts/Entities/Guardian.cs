using UnityEngine;

public class Guardian : Entity
{
    public override void MovementTurn()
    {
        print("Guardian movement");
    }

    public override void AttackTurn()
    {
        print("Guardian attack");
    }
}
