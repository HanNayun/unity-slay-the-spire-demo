using System.Collections.Generic;

public class DealDamageGA : GameAction
{
    public DealDamageGA(int damage, List<CombatantView> targets)
    {
        Damage = damage;
        Targets = targets;
    }

    public int Damage { get; private set; }
    public List<CombatantView> Targets { get; private set; }
}