using UnityEngine;

namespace ATH.InventorySystem
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Chest : MonoBehaviour, IInventoryHolder
    {
        [SerializeField]
        private Inventory _inventory;

        public Inventory Inventory
        {
            get => _inventory;
            set => _inventory = value;
        }

        [ContextMenu("Log Gold")]
        public void LogGold()
        {
            Debug.Log(_inventory.Gold);
        }

        public void ResetInventory()
        {
            _inventory = new Inventory();
        }
    }
}