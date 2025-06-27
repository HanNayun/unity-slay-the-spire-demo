using _Scripts.Data;
using _Scripts.General;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Creators
{
public class EnemyCreator : Singleton<EnemyCreator>
{
    [SerializeField]
    private EnemyView enemyPrefab;

    public EnemyView CreateEnemy(EnemyData data, Vector3 position, Quaternion rotation)
    {
        EnemyView enemyView = Instantiate(enemyPrefab);
        enemyView.Setup(data);
        enemyView.transform.position = position;
        enemyView.transform.rotation = rotation;
        return enemyView;
    }
}
}