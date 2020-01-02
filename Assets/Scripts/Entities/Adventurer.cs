using UnityEngine;

public class Adventurer : Entity
{
    public int mana;

    public override void MovementTurn()
    {
        print("Adventurer movement");
        MoveTo(new Vector2(6, 6));
    }

    public override void AttackTurn()
    {
        print("Adventurer attack");
        Attack(FindObjectOfType<Aspect>());
        gameManager.NextTurn();
    }
}
