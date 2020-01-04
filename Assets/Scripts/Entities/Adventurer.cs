using UnityEngine;

public class Adventurer : Entity
{
    public const int MAXMANA = 100;
    private int mana = MAXMANA - 40;
    public int Mana
    {
        get
        {
            return mana;
        }
        set
        {
            mana = value < 0 ? 0 : (mana > MAXMANA ? MAXMANA : value);
            GameManager.instance.UpdateManaBar(value);
        }
    }

    public override void MovementTurn()
    {
        print("Adventurer movement");
        Mana += 10;
    }

    public override void AttackTurn()
    {
        base.AttackTurn();
        print("Adventurer attack");
    }
}
