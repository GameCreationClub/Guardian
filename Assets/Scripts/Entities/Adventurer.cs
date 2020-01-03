using UnityEngine;

public class Adventurer : Entity
{
    public int mana;

    public void Start()
    {
        Camera.main.GetComponent<CameraMovement>().trackedObject = gameObject;
    }
    public override void MovementTurn()
    {
        print("Adventurer movement");
        Camera.main.GetComponent<CameraMovement>().trackedObject = gameObject;
    }

    public override void AttackTurn()
    {
        print("Adventurer attack");
    }
}
