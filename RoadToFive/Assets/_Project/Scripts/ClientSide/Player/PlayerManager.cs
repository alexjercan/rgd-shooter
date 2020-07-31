using _Project.Scripts.Mechanics;
using UnityEngine;

namespace _Project.Scripts.ClientSide.Player
{
    /// <summary>
    /// PLAYER MANAGER CONTINE TOATE COMPONENTELE CARE DEFINESC UN JUCATOR
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private EntityHealth entityHealth;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private HandWeapon handWeapon;

        private int _id;
        private string _username;

        private Transform _transform;

        private void Awake() => _transform = GetComponent<Transform>();

        public void Initialize(int id, string username)
        {
            _id = id;
            _username = username;
            entityHealth.Died += (sender, args) =>
            {
                gameObject.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            };
        }

        public void SetPosition(Vector3 position) => _transform.position = position;

        public void SetRotation(Quaternion rotation) => _transform.rotation = rotation;

        public void SetHealth(int health) => entityHealth.SetHealth(health);

        public bool HasAmmo() => playerInventory.GetAmmo() > 0;
        
        public void AddAmmo(int amount) => playerInventory.AddAmmo(amount);

        public void RemoveAmmo(int amount) => playerInventory.AddAmmo(-amount);

        public void AddWeapon(int weaponId) => playerInventory.AddWeapon(weaponId);

        public int GetWeaponCount() => playerInventory.GetWeaponCount();
        
        public void SetWeaponTo(int index) => handWeapon.SetWeaponTo(playerInventory.GetWeaponAtIndex(index));
    }
}
