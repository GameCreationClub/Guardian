using UnityEngine;

public class Guardian : Entity
{
    public override void MovementTurn()
    {
        print("Guardian movement");
        Camera.main.GetComponent<CameraMovement>().trackedObject = gameObject;

    }

    public override void AttackTurn()
    {
        base.AttackTurn();
        print("Guardian attack");
    }
}
