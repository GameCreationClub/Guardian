using UnityEngine;

public class Gloom : Entity
{
    public override void MovementTurn()
    {
        print("Gloom movement");
        MoveTo(new Vector2(7, 7));
    }

    public override void AttackTurn()
    {
        print("Gloom attack");
        GameManager.instance.NextTurn();
    }
}
