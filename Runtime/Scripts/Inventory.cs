using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using ATH.Models.Save;
using System.Text;
using System;

namespace ATH.InventorySystem
{
    /// <summary>
    /// 
    /// </summary>
    public class Inventory
    {
        private const int kDefaultWitdh = 4;
        private const int kDefaultHeight = 4;

        [SerializeField] private int _gold;

        private List<InventorySlot> _slots;
        private InventorySlot[,] _slotsMatrix;

        private int _witdh;
        private int _height;

        public int Width => _witdh;
        public int Height => _height;

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
        public event EventHandler OnSlotUpdate;

        public InventorySlot this[int x, int y]
        {
            get => _slotsMatrix[x, y];
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Inventory()
        {
            _gold = 0;
            _slots = new List<InventorySlot>();
            _slotsMatrix = new InventorySlot[kDefaultWitdh, kDefaultHeight];

            for (int i = 0; i < kDefaultWitdh; i++)
            {
                for (int j = 0; j < kDefaultHeight; j++)
                {
                    var slot = new InventorySlot();
                    _slots.Add(slot);
                    _slotsMatrix[i, j] = slot;
                }
            }
            _height = kDefaultHeight;
            _witdh = kDefaultWitdh;
        }

        /// <summary>
        /// Constructor that creates an inventory with a certain number of slots
        /// </summary>
        public Inventory(int width, int height)
        {
            _gold = 0;
            _slots = new List<InventorySlot>();
            _slotsMatrix = new InventorySlot[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var slot = new InventorySlot();
                    _slots.Add(slot);
                    _slotsMatrix[i, j] = slot;
                }
            }
            _witdh = width;
            _height = height;
        }

        /// <summary>
        /// Constructor that creates an inventory with a certain number of slots
        /// </summary>
        public Inventory(int[,] lockMatrix)
        {
            var width = lockMatrix.GetLength(0);
            var height = lockMatrix.GetLength(1);
            _gold = 0;
            _slots = new List<InventorySlot>();
            _slotsMatrix = new InventorySlot[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var slot = new InventorySlot(lockMatrix[i, j] == 0 ? InventoySlotState.Locked : InventoySlotState.Empty);
                    _slots.Add(slot);
                    _slotsMatrix[i, j] = slot;
                }
            }
            _witdh = width;
            _height = height;
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
            inventorySaveData.inventorySlotSaveDatas.ForEach(item => _slots.Add(new InventorySlot(new ItemEntry(itm, item.ammount))));
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
                if (_slots[i].State == InventoySlotState.Locked) continue;

                if (_slots[i].State == InventoySlotState.Empty)
                {
                    amountCopy -= item.StackLimit;
                    if (amountCopy <= 0) return true;
                    continue;
                }

                amountCopy -= _slots[i].Entry.GetFreeSpace(item);

                if (_slots[i].State == InventoySlotState.Empty)
                    amountCopy -= item.StackLimit;

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
                if (_slots[i].State == InventoySlotState.Empty) continue;
                if (_slots[i].State == InventoySlotState.Locked) continue;

                amountCopy -= _slots[i].Entry.Item == item ? _slots[i].Entry.Amount : 0;
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
                if (_slots[i].State == InventoySlotState.Locked) continue;

                if (_slots[i].State == InventoySlotState.Empty)
                {
                    _slots[i].Entry = new ItemEntry(item, 0);
                    _slots[i].State = InventoySlotState.Ocupied;
                    OnSlotUpdate?.Invoke(this, new InventoryUpdateArgs() { Operation = InventoryUpdateArgs.OperationType.Increase, Slot = _slots[i] });
                }

                var currentSlotAmount = _slots[i].Entry.GetFreeSpace(item);

                if (currentSlotAmount > 0)
                {
                    _slots[i].Entry.IncreaseAmount(item, Mathf.Min(amountCopy, currentSlotAmount));
                    OnSlotUpdate?.Invoke(this, new InventoryUpdateArgs() { Operation = InventoryUpdateArgs.OperationType.Increase, Slot = _slots[i] });
                    amountCopy -= currentSlotAmount;
                }

                if (amountCopy <= 0)
                {
                    return;
                }
            }

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
                if (_slots[i].State == InventoySlotState.Empty) continue;
                if (_slots[i].State == InventoySlotState.Locked) continue;

                var correctItem = _slots[i].Entry.Item == item;
                if (!correctItem) continue;

                var currentSlotAmount = _slots[i].Entry.Amount;

                if (currentSlotAmount > 0)
                {
                    _slots[i].Entry.DecreaseAmount(Mathf.Min(amountCopy, currentSlotAmount));
                    OnSlotUpdate?.Invoke(this, new InventoryUpdateArgs() { Operation = InventoryUpdateArgs.OperationType.Decrease, Slot = _slots[i] });
                    amountCopy -= currentSlotAmount;
                }

                if (_slots[i].Entry.Amount <= 0)
                {
                    _slots[i].Entry = null;
                    _slots[i].State = InventoySlotState.Empty;
                    OnSlotUpdate?.Invoke(this, new InventoryUpdateArgs() { Operation = InventoryUpdateArgs.OperationType.Decrease, Slot = _slots[i] });
                }

                if (amountCopy <= 0)
                {
                    return;
                }

            }
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
                if (slot.State == InventoySlotState.Empty) continue;
                if (slot.State == InventoySlotState.Locked) continue;

                if (slot.Entry.Item == item)
                {
                    currentAmmount += slot.Entry.Amount;
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
                if (item.Entry != null)

                    item.Reset();
            }
            OnSlotUpdate?.Invoke(this, new InventoryUpdateArgs() { Operation = InventoryUpdateArgs.OperationType.Reset, Slot = null });
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
            foreach (var slot in _slots)
            {
                if (slot.State == InventoySlotState.Empty) continue;
                if (slot.State == InventoySlotState.Locked) continue;

                retVal.inventorySlotSaveDatas.Add(new InventorySlotSaveData()
                {
                    id = slot.Entry.Item.ItemName.GetHashCode(),
                    ammount = slot.Entry.Amount
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

        public void SetLockStateOfSlot(int x, int y)
        {
            this[x, y].Reset();
            this[x, y].State = InventoySlotState.Locked;
            OnSlotUpdate?.Invoke(this, new InventoryUpdateArgs() { Operation = InventoryUpdateArgs.OperationType.Lock, Slot = this[x, y] });
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
