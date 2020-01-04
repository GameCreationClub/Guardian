using UnityEngine;

public class Prowler : Enemy
{
    public override void MovementTurn()
    {
        MoveTo(new Vector2(6, 7));
    }

    public override void AttackTurn()
    {
        //base.AttackTurn();
        GameManager.instance.NextTurn();
    }
}
