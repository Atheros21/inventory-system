using System;
using UnityEngine;

namespace ATH.InventorySystem
{
    [CreateAssetMenu]
    public class UIWrapper : ScriptableObject
    {
        public event Action<Inventory> OnChestOpen;
        public event Action OnChestClose;

        public void FireChestClose()
        {
            OnChestClose?.Invoke();
        }

        public void FireChestOpen(Inventory inventory)
        {
            OnChestOpen?.Invoke(inventory);
        }
    } 
}