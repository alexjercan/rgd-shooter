using _Project.Scripts.ClientSide.Networking;
using _Project.Scripts.Mechanics;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC UN JUCATOR
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        public new Transform transform;
        public EntityHealth entityHealth;
        public PlayerInventory playerInventory;
        public HandWeapon handWeapon;

        private int _id;
        private string _username;

        public void Initialize(int id, string username)
        {
            _id = id;
            _username = username;
            entityHealth.Died += (sender, args) =>
            {
                gameObject.SetActive(false);
                if (Client.MyId != _id) return;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            };
        }
    }
}
