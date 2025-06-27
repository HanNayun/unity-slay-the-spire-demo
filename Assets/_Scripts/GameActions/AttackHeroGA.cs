public class AttackHeroGA : GameAction
{
    public AttackHeroGA(EnemyView attacker)
    {
        Attacker = attacker;
    }

    public EnemyView Attacker { get; private set; }
}