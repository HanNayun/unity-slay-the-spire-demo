using System;
using System.Collections;
using UnityEngine;

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
        foreach (var target in ga.Targets)
        {
            target.Damage(ga.Damage);
            Instantiate(damageVFX, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(.15f);
        }
    }
}