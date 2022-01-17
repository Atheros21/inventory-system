using System;

namespace ATH.InventorySystem
{
    /// <summary>
    /// Class that is responsible for an inventory slot.
    /// It holds an item and an amount of said item.
    /// Can perform operation on the amount of item.
    /// </summary>
    [Serializable]
    public class InventorySlot
    {
        private Item _item;
        private int _amount;

        /// <summary>
        /// The item from the slot
        /// </summary>
        public Item Item => _item;

        /// <summary>
        /// The ammount of the item from the slot
        /// </summary>
        public int Amount => _amount;

        /// <summary>
        /// Gets if the the item is empty
        /// </summary>
        public bool IsEmpty => _amount == 0 || _item == null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventorySlot()
        {
            _amount = 0;
            _item = null;
        }

        /// <summary>
        /// Constror with class parameters
        /// </summary>
        public InventorySlot(Item item, int amount)
        {
            if (amount <= 0 || item == null)
            {
                _amount = 0;
                _item = null;
                return;
            }

            _item = item;
            _amount = amount;
        }

        /// <param name="item">The item that will be checked for free space.</param>
        /// <returns>The amount of an item that can be added to this slot.</returns>
        public int GetFreeSpace(Item item)
        {
            return _item == null ? item.StackLimit : item.StackLimit - _amount;
        }

        /// <summary>
        /// Increases the amount of an item in this slot.
        /// </summary>
        /// <param name="item">The item to be added in this slot.
        /// If the slot has an item, the item param needs to be the same as is in the slot</param>
        /// <param name="amount">The amount of item to be added to the slot</param>
        public void IncreaseAmount(Item item, int amount)
        {
            if (_item == null)
            {
                _item = item;
            }

            if (amount < 0 || amount > GetFreeSpace(item))
            {
                throw new ArgumentException("Failed to increase the amount for an item, the amount is negative or is larger than the slot capacity");
            }

            if (_item != item)
            {
                throw new ArgumentException("Tried to increase a slots with an item that is diffrent than the one that is currently in it");
            }

            _amount += amount;
        }

        /// <summary>
        /// Decreases the amount of an item in this slot.
        /// </summary>
        /// <param name="amount">The amount of item to be removed from slot</param>
        public void DecreaseAmount(int amount)
        {
            if (amount < 0 || amount > _amount || IsEmpty)
            {
                throw new ArgumentException("Failed to decrease amount fon an item, the amount is negative, too large or slot is empty");
            }

            _amount -= amount;

            if (_amount <= 0)
            {
                _item = null;
            }
        }

        /// <summary>
        /// Rests the data in the slot
        /// </summary>
        public void Reset()
        {
            _item = null;
            _amount = 0;
        }
    }
}