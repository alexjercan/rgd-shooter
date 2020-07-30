namespace _Project.Scripts.Util.Weapon
{
    public class Weapon
    {
        public int Damage => _weaponScriptableObject.damage;
        
        private readonly WeaponScriptableObject _weaponScriptableObject;
        private int _ammoCount;

        public Weapon(WeaponScriptableObject weapon)
        {
            _weaponScriptableObject = weapon;
            _ammoCount = 0;
        }

        public void RemoveAmmo(int count) => _ammoCount -= count;

        public void AddAmmo(int count) => _ammoCount += count;

    }
}