using _Project.Scripts.ServerSide.Networking;
using _Project.Scripts.ServerSide.Player;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Item
{
    public class ItemSpawnerManager : MonoBehaviour
    {
        public static void OnItemSpawned(int spawnerId) => ServerSend.ItemSpawned(spawnerId);

        public static bool OnTryPickUpItem(int spawnerId, Collider other)
        {
            if (!other.CompareTag("Player")) return false;
            
            var playerManager = other.GetComponent<ServerPlayerManager>();
            ServerSend.ItemPickedUp(spawnerId, playerManager.Id);

            return true;
        }
    }
}