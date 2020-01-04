using UnityEngine;

public class Aspect : Entity
{
    public override void MovementTurn()
    {
        print("Aspect movement");
        MoveTo(new Vector2(8, 7));
    }

    public override void AttackTurn()
    {
        //base.AttackTurn();
        print("Aspect attack");
        GameManager.instance.NextTurn();
    }
}
