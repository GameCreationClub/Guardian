public class Gloom : Entity
{
    public override void MovementTurn()
    {
        print("Gloom movement");
        gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Gloom attack");
        gameManager.NextTurn();
    }
}
