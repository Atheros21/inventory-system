using UnityEngine;
using UnityEngine.UI;

namespace ATH.InventorySystem.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private Sprite _lockSlot;
        [SerializeField] private Sprite _unlockSlot;
        [SerializeField] private ItemEntryUI _itemEntryPrefab;
        [Header("Refrences")]
        [SerializeField] private Image _slotBg;
        [SerializeField] private Transform _itemEntryParent;

        private InventorySlot _inventorySlot;
        private ItemEntryUI _instantiatedItemEntryUI;
        private Canvas _canvasWhileDragging;
        private Canvas _canvas;

        public void Bind(InventorySlot inventorySlot, Canvas parentCanvs, Canvas canvasWhileDragging)
        {
            _inventorySlot = inventorySlot;
            _canvas = parentCanvs;
            _canvasWhileDragging = canvasWhileDragging;
        }

        public void Redraw()
        {
            if (_inventorySlot == null) return;

            _slotBg.sprite = _inventorySlot.State == InventoySlotState.Locked ? _lockSlot : _unlockSlot;

            if (_inventorySlot.Entry == null) return;

            if (_instantiatedItemEntryUI == null)
            {
                _instantiatedItemEntryUI = Instantiate(_itemEntryPrefab, _itemEntryParent);
                _instantiatedItemEntryUI.SetParentCanvas(_canvas);
                _instantiatedItemEntryUI.SetCanvasWhileDragging(_canvasWhileDragging);
            }

            _instantiatedItemEntryUI.UpdateEntry(_inventorySlot.Entry);
        }
    }
}
