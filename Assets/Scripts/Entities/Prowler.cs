public class Prowler : Entity
{
    public override void MovementTurn()
    {
        print("Prowler movement");
        gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Prowler attack");
        gameManager.NextTurn();
    }
}
