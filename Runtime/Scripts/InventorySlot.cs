using System;

namespace ATH.InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        public InventoySlotState State;
        public ItemEntry Entry;

        /// <summary>
        /// Creates an empty inventory slot with null item entry
        /// </summary>
        public InventorySlot()
        {
            Entry = null;
            State = InventoySlotState.Empty;
        }

        /// <summary>
        /// Creates an empty slots with the put state, use for empty state or ocupied state only,
        /// Null item entry.
        /// </summary>
        public InventorySlot(InventoySlotState state)
        {
            if(state == InventoySlotState.Ocupied)
            {
                throw new ArgumentException("Constructor intedended for empty and lock state");
            }

            Entry = null;
            State = state;
        }

        /// <summary>
        /// Creates an inventroy slot with the put item entry. The state will be ocupied.
        /// The item entry does not accept null.
        /// </summary>
        public InventorySlot(ItemEntry entry)
        {
            Entry = entry ?? throw new ArgumentException("Null entry for a ocupied inventory slot");
            State = InventoySlotState.Ocupied;
        }

        /// <summary>
        /// Rests the data in the slot
        /// </summary>
        public void Reset()
        {
            Entry = null;
            State = InventoySlotState.Empty;
        }
    }
}