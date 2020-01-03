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
        print("Guardian attack");
    }
}
