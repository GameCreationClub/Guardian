using UnityEngine;

public class Guardian : Entity
{
    public void Start()
    {
        Camera.main.GetComponent<CameraMovement>().trackedObject = gameObject;//this is temp later it would make more sense for the game manager to actually set this when needed
    }
    public override void MovementTurn()
    {
        print("Guardian movement");
    }

    public override void AttackTurn()
    {
        print("Guardian attack");
    }
}
