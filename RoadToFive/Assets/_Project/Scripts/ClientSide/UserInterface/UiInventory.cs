using _Project.Scripts.ClientSide.LocalPlayer;
using _Project.Scripts.ClientSide.Player;
using _Project.Scripts.Util.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerShootInput _weaponInHand;
    [SerializeField] private List<Image> _inventoryPictures;
    [SerializeField] private List<Image> _inventoryCells;
    [SerializeField] private Color _inactiveInventoryPic;
    [SerializeField] private Color _selectedInventoryPic;
    private int previousItem;
    void Start()
    {
        previousItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int count = _inventory.GetWeaponCount();
        for (int index = 0; index < count; ++index)
        {
            ItemScriptableObject weapon = _inventory.GetWeaponAtIndex(index);
            _inventoryPictures[index].sprite = weapon.inventoryPic;
        }

        int currentItem = _weaponInHand.GetWeaponIndex(count);
        if (previousItem != currentItem)
        {
            _inventoryCells[previousItem].color = _inactiveInventoryPic;
            _inventoryCells[currentItem].color = _selectedInventoryPic;
        }
    }
}
