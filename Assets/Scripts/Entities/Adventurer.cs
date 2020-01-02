using UnityEngine;

public class Adventurer : Entity
{
    public int mana;

    public override void MovementTurn()
    {
        print("Adventurer movement");
        gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Adventurer attack");
        gameManager.NextTurn();
    }
}
