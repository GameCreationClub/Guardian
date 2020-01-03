using UnityEngine;

public class Adventurer : Entity
{
    public const int MAXMANA = 100;
    private int mana = MAXMANA-40;
    public int Mana{
        get{
            return mana;
        }
        set{
            mana = value < 0 ? 0 : (mana > MAXMANA ? MAXMANA : value);
            GameManager.instance.UpdateManaBar(value);
        }
    }

    public void Start()
    {
        Camera.main.GetComponent<CameraMovement>().trackedObject = gameObject;
    }
    public override void MovementTurn()
    {

        print("Adventurer movement");
        Mana = Mana + 10;
    }
    
    public override void AttackTurn()
    {
        print("Adventurer attack");
    }
}
