using System.Collections;
using _Scripts.GameActions;
using _Scripts.General;
using _Scripts.General.ActionSystem;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Systems
{
    public class DamageSystem : Singleton<DamageSystem>
    {
        [SerializeField]
        private GameObject damageVFX;

        private void OnEnable()
        {
            ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<DealDamageGA>();
        }

        private IEnumerator DealDamagePerformer(DealDamageGA ga)
        {
            foreach (CombatantView target in ga.Targets)
            {
                target.Damage(ga.Damage);
                Instantiate(damageVFX, target.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(.15f);

                if (target.CurrentHealth <= 0)
                {
                    if (target is EnemyView enemy)
                    {
                        ActionSystem.Instance.AddReaction(new KillEnemyGA(enemy));
                    }
                    else
                    {
                    }
                }
            }
        }
    }
}