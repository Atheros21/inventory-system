using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using ATH.Models.Save;

namespace ATH.InventorySystem
{
    /// <summary>
    /// 
    /// </summary>
    public class Inventory
    {
        private const int kDefaultInventorySize = 10;

        [SerializeField] private int _gold;

        private List<InventorySlot> _slots;

        /// <summary>
        /// Gets the gold amount from the inventory.
        /// </summary>
        public int Gold => _gold;

        /// <summary>
        /// Event invoked when the gold ammount from this inventory is changed.
        /// </summary>
        public UnityEvent OnGoldUpdate = new UnityEvent();

        /// <summary>
        /// Event invoked when a item in the inventory is updated.
        /// </summary>
        public UnityEvent OnItemUpdate = new UnityEvent();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Inventory()
        {
            _gold = 0;
            _slots = new List<InventorySlot>();
            for (int i = 0; i < kDefaultInventorySize; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }

        /// <summary>
        /// Inventory contructor using save data.
        /// </summary>
        public Inventory(InventorySaveData inventorySaveData)
        {
            _gold = inventorySaveData.money;
            _slots = new List<InventorySlot>();
            //GET ITEM USING ID
            Item itm = null;
            inventorySaveData.inventorySlotSaveDatas.ForEach(item => _slots.Add(new InventorySlot(itm,item.ammount)));
        }

        /// <summary>
        /// Add/Increases an item from the inventory.
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <param name="ammount">The ammount to be added</param>
        public void AddItem(Item item, int ammount)
        {
            
        }

        /// <summary>
        /// Removes/Decreases an item from the inventory.
        /// </summary>
        /// <param name="item">The item to be removed/decreased</param>
        /// <param name="ammount">The ammount to be decreased</param>
        public void RemoveItem(Item item, int ammount)
        {
          
        }

        /// <summary>
        /// Method to obtain the count of an item from the inventory
        /// </summary>
        /// <param name="item">The item to be cheked</param>
        /// <returns>Integer representing the count of the item</returns>
        public int GetItemCount(Item item)
        {
            int currentAmmount = 0;
            foreach (var slot in _slots)
            {
                if (slot.Item == item)
                {
                    currentAmmount += slot.Amount;
                }
            }
            return currentAmmount;
        }

        /// <summary>
        /// Empties the inventory
        /// </summary>
        public void ResetInvetory()
        {
            _gold = 0;
            foreach (var item in _slots)
            {
               item.Reset();
            }
        }

        /// <summary>
        /// Functions used to obtain data to be saved as json for progress.
        /// </summary>
        /// <returns>Struct that will be serialized for json</returns>
        public InventorySaveData GetInventorySaveData()
        {
            InventorySaveData retVal = new InventorySaveData();
            retVal.money = _gold;
            retVal.inventorySlotSaveDatas = new List<InventorySlotSaveData>();
            foreach (InventorySlot slot in _slots)
            {
                retVal.inventorySlotSaveDatas.Add(new InventorySlotSaveData() 
                { 
                    id = slot.Item.ItemName.GetHashCode(),
                    ammount = slot.Amount 
                });
            }
            return retVal;
        }

        /// <summary>
        /// Check if there is a certain amount of money in inventory
        /// </summary>
        /// <returns>True if it has the money</returns>
        public bool HasGold(int amount) => _gold >= amount;

        /// <summary>
        /// Increase or decreases the current ammount of money by the paramater targetAmmount
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeGold(int amount)
        {
            _gold += amount;
            OnGoldUpdate.Invoke();
        }
    }
}
