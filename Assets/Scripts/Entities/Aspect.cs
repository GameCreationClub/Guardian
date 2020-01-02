using UnityEngine;

public class Aspect : Entity
{
    public override void MovementTurn()
    {
        print("Aspect movement");
        MoveTo(new Vector2(8, 7));
        //gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Aspect attack");
        gameManager.NextTurn();
    }
}
