using UnityEngine;
using ATH.InventorySystem.UI;
using UnityEngine.UI;

namespace ATH.InventorySystem.DevelopTests
{
    public class InventroyDrawDevelop : MonoBehaviour
    {
        [SerializeField] private Item _item1;
        [SerializeField] private Item _item2;

        [SerializeField] private InventoryUI _inventoryUI;

        [SerializeField] private Button _openInventory;
        [SerializeField] private Button _inventory1;
        [SerializeField] private Button _inventory2;
        
        [SerializeField] private Button _addItem1;
        [SerializeField] private Button _addItem2;
        [SerializeField] private Button _removeItem1;

        [SerializeField] private Inventory _debugInventory;

        private void Awake()
        {
            _openInventory.onClick.AddListener(OpenInventory);
            _inventory1.onClick.AddListener(OpenInventory1);
            _inventory2.onClick.AddListener(OpenInventory2);
            _addItem1.onClick.AddListener(AddItem);
            _addItem2.onClick.AddListener(AddItem2);
            _removeItem1.onClick.AddListener(RemoveItem);
        }

        private void OpenInventory()
        {
            _inventoryUI.Open();
        }

        private void OpenInventory1()
        {
            _debugInventory = new Inventory(6, 4);
            _debugInventory.ChangeGold(100);
            _inventoryUI.SetInventory(_debugInventory);
            _inventoryUI.Draw();
            _inventoryUI.Open();
        }

        private void OpenInventory2()
        {
            _debugInventory = new Inventory(new int[,]
            {
                {0,1,1,1,1,0 },
                {0,1,1,1,1,0 },
                {1,1,1,1,1,1 },
                {1,1,1,1,1,1 },
            });
            _debugInventory.ChangeGold(10);
            _inventoryUI.SetInventory(_debugInventory);
            _inventoryUI.Draw();
            _inventoryUI.Open();
        }

        private void AddItem()
        {
            _debugInventory.AddItem(_item1, 1);
        }

        private void AddItem2()
        {
            _debugInventory.AddItem(_item2, 1);
        }

        private void RemoveItem()
        {
            _debugInventory.RemoveItem(_item1, 1);
        }

    }
}
