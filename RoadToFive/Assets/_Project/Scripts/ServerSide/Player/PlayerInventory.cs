using System.Collections.Generic;
using _Project.Scripts.Util.Weapon;
using UnityEngine;

namespace _Project.Scripts.ServerSide.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<Weapon> weapons;
    }
}