using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using ATH.Models.Save;
using System.Text;

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
        /// Retutns a list with all the inventory slots
        /// </summary>
        public List<InventorySlot> Slots => _slots;

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
        /// Constructor that creates an inventory with a certain number of slots
        /// </summary>
        public Inventory(int slotsCount)
        {
            _gold = 0;
            _slots = new List<InventorySlot>();
            for (int i = 0; i < slotsCount; i++)
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
            inventorySaveData.inventorySlotSaveDatas.ForEach(item => _slots.Add(new InventorySlot(itm, item.ammount)));
        }

        /// <summary>
        /// Check if it can add/increases an item from the inventory.
        /// </summary>
        /// <param name="item">The item to be checked</param>
        /// <param name="amount">The ammount to be checked</param>
        /// <returns>Boolean representing if the items can be added</returns>
        public bool CanAddItem(Item item, int amount)
        {
            var amountCopy = amount;

            for (var i = 0; i < _slots.Count; i++)
            {
                amountCopy -= _slots[i].GetFreeSpace(item);

                if (amountCopy <= 0) return true;
            }

            return false;
        }

        /// <summary>
        /// Check if it can remove/decrease an item from the inventory.
        /// </summary>
        /// <param name="item">The item to be checked</param>
        /// <param name="amount">The ammount to be checked</param>
        /// <returns>Boolean representing if the items can be removed</returns>
        public bool CanRemoveItem(Item item, int amount)
        {
            var amountCopy = amount;

            for (int i = 0; i < _slots.Count; i++)
            {
                amountCopy -= _slots[i].Item == item ? _slots[i].Amount : 0;
            }

            if (amountCopy <= 0) return true;

            return false;
        }

        /// <summary>
        /// Add/Increases an item from the inventory.
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <param name="amount">The ammount to be added</param>
        public void AddItem(Item item, int amount)
        {
            var amountCopy = amount;

            for (var i = 0; i < _slots.Count; i++)
            {
                var currentSlotAmount = _slots[i].GetFreeSpace(item);

                if (currentSlotAmount > 0)
                {
                    _slots[i].IncreaseAmount(item, Mathf.Min(amountCopy, currentSlotAmount));
                    amountCopy -= currentSlotAmount;
                }


                if (amountCopy <= 0)
                {
                    OnItemUpdate?.Invoke();
                    return;
                }

            }

            OnItemUpdate?.Invoke();
        }

        /// <summary>
        /// Removes/Decreases an item from the inventory.
        /// </summary>
        /// <param name="item">The item to be removed/decreased</param>
        /// <param name="ammount">The ammount to be decreased</param>
        public void RemoveItem(Item item, int amount)
        {
            var amountCopy = amount;

            for (var i = 0; i < _slots.Count; i++)
            {
                var correctItem = _slots[i].Item == item;
                if (!correctItem) continue;

                var currentSlotAmount = _slots[i].Amount;

                if (currentSlotAmount > 0)
                {
                    _slots[i].DecreaseAmount(Mathf.Min(amountCopy, currentSlotAmount));
                    amountCopy -= currentSlotAmount;
                }

                if (amountCopy <= 0)
                {
                    OnItemUpdate?.Invoke();
                    return;
                }

            }

            OnItemUpdate?.Invoke();
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

            OnItemUpdate?.Invoke();
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

        public void SetGold(int amount)
        {
            _gold = amount;
            OnGoldUpdate.Invoke();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Size: [{_slots.Count}]\n");
            stringBuilder.Append($"Gold: [{Gold}]");

            for (int i = 0; i < _slots.Count; i++)
            {
                stringBuilder.Append($"Slot[{i}]: [{_slots[i]}]\n");
            }

            return stringBuilder.ToString();
        }
    }
}
