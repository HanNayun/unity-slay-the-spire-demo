using System.Diagnostics.CodeAnalysis;
using _Scripts.General;
using _Scripts.Views;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.Systems
{
    public class ManualTargetSystem : Singleton<ManualTargetSystem>
    {
        [SerializeField]
        private ArrowView arrowView;

        [SerializeField]
        private LayerMask targetLayerMask;

        public void StartTargeting(Vector3 startPos)
        {
            arrowView.gameObject.SetActive(true);
            arrowView.Setup(startPos);
        }

        public bool EndTargeting(Vector3 endPos, [NotNullWhen(true), CanBeNull] out EnemyView enemy)
        {
            arrowView.gameObject.SetActive(false);
            if (Physics.Raycast(endPos, Vector3.forward, out RaycastHit hit, 10f, targetLayerMask) &&
                hit.collider != null &&
                hit.collider.gameObject.TryGetComponent(out EnemyView enemyView))
            {
                enemy = enemyView;
                return true;
            }

            enemy = null;
            return false;
        }
    }
}