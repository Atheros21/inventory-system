using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace ATH.InventorySystem.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private InventorySlotUI _inventorySlotPrefab;
        [Header("Refrences")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Canvas _canvasWhileDragging;
        [SerializeField] private TextMeshProUGUI _coins;
        [SerializeField] private Button _close;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private GameObject _root;

        private Inventory _inventory;
        private Dictionary<int, InventorySlotUI> _instantiatedSlots;


        private void Awake()
        {
            _instantiatedSlots = new Dictionary<int, InventorySlotUI>();

            if (_close != null)
                _close.onClick.AddListener(() => _root.SetActive(false));
        }

        public void SetInventory(Inventory inventory)
        {
            if(_inventory != null)
            {
                _inventory.OnSlotUpdate -= SlotUpdated;
            }

            _inventory = inventory;
            _inventory.OnSlotUpdate += SlotUpdated;
        }

        public void Draw()
        {
            if (_inventory == null) throw new NullReferenceException("Can't draw. Inventory null.");

            ClearAllSlots();
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                var slotClone = Instantiate(_inventorySlotPrefab, _slotsContainer);
                slotClone.Bind(_inventory.Slots[i], _canvas, _canvasWhileDragging);
                slotClone.Redraw();
                _instantiatedSlots.Add(_inventory.Slots[i].GetHashCode(), slotClone);
            }

            if (_coins != null)
                _coins.SetText(_inventory.Gold.ToString());
        }

        public void Open()
        {
            _root.SetActive(true);
        }

        public void Close()
        {
            _root.SetActive(false);
        }

        private void ClearAllSlots()
        {
            foreach (var item in _instantiatedSlots)
            {
                Destroy(item.Value.gameObject);
            }

            _instantiatedSlots.Clear();
        }

        private void SlotUpdated(object sender, EventArgs args)
        {
            var eventData = (InventoryUpdateArgs)args;

            if (eventData.Slot == null) return;

            var hash = eventData.Slot.GetHashCode();

            if (!_instantiatedSlots.ContainsKey(hash)) return;

            _instantiatedSlots[hash].Redraw();
        }
    }
}