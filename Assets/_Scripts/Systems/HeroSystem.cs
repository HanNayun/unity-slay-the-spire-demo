using _Scripts.Data;
using _Scripts.General;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Systems
{
public class HeroSystem : Singleton<HeroSystem>
{
    [SerializeField]
    public HeroView HeroView;

    public void Setup(HeroData data)
    {
        HeroView.Setup(data);
    }
}
}