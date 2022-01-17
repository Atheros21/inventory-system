using System.Collections.Generic;

namespace ATH.Models.Save
{
    /// <summary>
    /// Holds the save data for an inventory
    /// </summary>
    public struct InventorySaveData
    {
        public int id;
        public int money;
        public List<InventorySlotSaveData> inventorySlotSaveDatas;
    }

    /// <summary>
    /// Holds the save data for a single inventoy slot
    /// </summary>
    public struct InventorySlotSaveData
    {
        public int id;
        public int ammount;
    }
}