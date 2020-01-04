using UnityEngine;

public class Aspect : Enemy
{
    public override void MovementTurn()
    {
        MoveTo(new Vector2(8, 7));
    }

    public override void AttackTurn()
    {
        //base.AttackTurn();
        GameManager.instance.NextTurn();
    }
}
