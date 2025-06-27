using _Scripts.Data;

namespace _Scripts.Views
{
public class HeroView : CombatantView
{
    public void Setup(HeroData heroData)
    {
        SetupBase(heroData.Health, heroData.Image);
    }
}
}