using UnityEngine;

namespace ATH.InventorySystem
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Chest : MonoBehaviour
    {
        private Inventory _inventory;

        public Inventory Inventory => _inventory;

        public void ResetInventory()
        {
            _inventory = new Inventory();
        }
    }
}