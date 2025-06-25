using _Scripts.Data;
using UnityEngine;

public class EnemyCreator : Singleton<EnemyCreator>
{
    [SerializeField]
    private EnemyView enemyPrefab;

    public EnemyView CreateEnemy(EnemyData data, Vector3 position, Quaternion rotation)
    {
        var enemyView = Instantiate(enemyPrefab);
        enemyView.Setup(data);
        enemyView.transform.position = position;
        enemyView.transform.rotation = rotation;
        return enemyView;
    }
}