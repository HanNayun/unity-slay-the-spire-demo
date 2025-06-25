using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    [SerializeField]
    private List<Transform> slots;

    public List<EnemyView> EnemyViews { get; set; } = new();

    public void AddEnemy(EnemyData enemyData)
    {
        var slot = slots[EnemyViews.Count];
        var enemyView = EnemyCreator.Instance.CreateEnemy(enemyData, slot.position, slot.rotation);
        enemyView.transform.SetParent(slot);
        EnemyViews.Add(enemyView);
    }
}