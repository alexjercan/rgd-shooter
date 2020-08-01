using UnityEngine;

namespace _Project.Scripts.Util.Enemy
{
    [CreateAssetMenu(fileName = "new enemy", menuName = "Entity/Enemy", order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        public int Id { get; set; }
        public ClientSide.Enemy.EnemyManager clientPrefab;
        public ServerSide.Enemy.EnemyManager serverPrefab;
    }
}