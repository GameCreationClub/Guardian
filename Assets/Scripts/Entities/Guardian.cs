public class Guardian : Entity
{
    public override void MovementTurn()
    {
        print("Guardian movement");
        gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Guardian attack");
        gameManager.NextTurn();
    }
}
