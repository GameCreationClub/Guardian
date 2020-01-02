public class Aspect : Entity
{
    public override void MovementTurn()
    {
        print("Aspect movement");
        gameManager.NextTurn();
    }

    public override void AttackTurn()
    {
        print("Aspect attack");
        gameManager.NextTurn();
    }
}
