using System;

namespace ATH.InventorySystem
{
    public class InventoryUpdateArgs : EventArgs
    {
        public InventorySlot Slot;
        public OperationType Operation;


        public enum OperationType
        { 
            Decrease,
            Increase,
            Reset,
            Lock
        }
    } 
}
