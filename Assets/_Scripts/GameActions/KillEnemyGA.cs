public class KillEnemyGA : GameAction
{
    public KillEnemyGA(EnemyView target)
    {
        Target = target;
    }

    public EnemyView Target { get; private set; }
}