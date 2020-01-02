using UnityEngine;

public class Adventurer : Entity
{
    public int mana;

    public override void MovementTurn()
    {
        print("Adventurer movement");
    }

    public override void AttackTurn()
    {
        print("Adventurer attack");
    }
}
