using UnityEngine;

public class Prowler : Entity
{
    public override void MovementTurn()
    {
        print("Prowler movement");
        MoveTo(new Vector2(6, 7));
        //gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Prowler attack");
        gameManager.NextTurn();
    }
}
