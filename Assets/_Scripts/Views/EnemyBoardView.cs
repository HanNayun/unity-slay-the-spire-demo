using System.Collections;
using System.Collections.Generic;
using _Scripts.Creators;
using _Scripts.Data;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Views
{
public class EnemyBoardView : MonoBehaviour
{
    [SerializeField]
    private List<Transform> slots;

    public List<EnemyView> EnemyViews { get; private set; } = new();

    public void AddEnemy(EnemyData enemyData)
    {
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyCreator.Instance.CreateEnemy(enemyData, slot.position, slot.rotation);
        enemyView.transform.SetParent(slot);
        EnemyViews.Add(enemyView);
    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        EnemyViews.Remove(enemyView);
        yield return enemyView.transform.DOScale(Vector3.zero, .15f).WaitForCompletion();
        Destroy(enemyView.gameObject);
    }
}
}