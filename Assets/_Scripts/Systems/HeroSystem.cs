using _Scripts.Data;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [SerializeField]
    public HeroView HeroView;

    public void Setup(HeroData data)
    {
        HeroView.Setup(data);
    }
}