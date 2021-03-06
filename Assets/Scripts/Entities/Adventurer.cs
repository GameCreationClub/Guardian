﻿using UnityEngine;

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
        Mana += 10;
    }

    public override void AttackTurn()
    {
        base.AttackTurn();
        //GameManager.instance.NextTurn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit"))
            GameManager.instance.PlayerExit();
    }
}
