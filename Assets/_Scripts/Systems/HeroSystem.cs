using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [SerializeField]
    private HeroView heroView;

    public void Setup(HeroData data)
    {
        heroView.Setup(data);
    }
}