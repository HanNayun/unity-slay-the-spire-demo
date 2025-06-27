using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.General.ActionSystem;
using _Scripts.Interfaces;
using _Scripts.Systems;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Models
{
    public class Perk
    {
        private readonly PerkData perkData;
        private readonly PerkCondition condition;
        private readonly AutoTargetEffect effect;

        public Perk(PerkData perkData)
        {
            this.perkData = perkData;
            effect = perkData.AutoTargetEffect;
            condition = perkData.PerkCondition;
        }

        public Sprite Image => perkData.Image;

        public void OnAdd()
        {
            condition.SubscribeCondition(Reaction);
        }

        public void OnRemove()
        {
            condition.UnsubscribeCondition(Reaction);
        }

        private void Reaction(GameAction gameAction)
        {
            if (!condition.IsSubconditionMet(gameAction)) return;

            List<CombatantView> targets = new();
            if (perkData.IsUseCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets.Add(haveCaster.Caster);
            }

            if (perkData.IsUseAutoTarget)
            {
                targets.AddRange(effect.TargetMode.GetTargets());
            }

            GameAction perkEffectAction = effect.Effect.GetGameAction(targets, HeroSystem.Instance.HeroView);
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }
}