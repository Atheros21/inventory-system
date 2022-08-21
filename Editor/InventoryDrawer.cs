using UnityEngine.UIElements;

namespace ATH.InventorySystem
{
    public class InventoryDrawer
    {
        private VisualTreeAsset _inventoryVisualTreeAsset;
        private VisualTreeAsset _inventorySlotTreeAsset;
        private Inventory _inventory;
        private VisualElement _root;

        public VisualElement Root => _root;

        public InventoryDrawer(Inventory inventory)
        {
            _inventoryVisualTreeAsset = UXMLSet.GetUxmlSetInstance().Inventory;
            _inventorySlotTreeAsset = UXMLSet.GetUxmlSetInstance().InventorySlot;
            _inventory = inventory;
            CreateNewRoot();

            //if (inventory != null)
                //_inventory.OnItemUpdate.AddListener(CreateNewRoot);
        }

        public InventoryDrawer(Inventory inventory, VisualElement extisingDrawer)
        {
            _inventorySlotTreeAsset = UXMLSet.GetUxmlSetInstance().InventorySlot;
            _inventory = inventory;
            _root = extisingDrawer;
            UpdateVisuals(_root);
        }

        private void CreateNewRoot()
        {
            var root = new VisualElement();
            root.Clear();
            _inventoryVisualTreeAsset.CloneTree(root);

            _root = root;
        }

        public void UpdateVisuals(VisualElement root)
        {
            if (_inventory == null)
            {
                root.Q<Label>("gold").text = "Null inventory";
                return;
            }

            root.Q<Label>("gold").text = $"Gold: {_inventory.Gold}";

            var slots = root.Q<Foldout>("slots");
            slots.Clear();
            foreach (var slotData in _inventory.Slots)
            {
                var slotElement = new VisualElement();
                _inventorySlotTreeAsset.CloneTree(slotElement);
                var item = slotData.Entry.Item;

                slotElement.Q<Label>("Name").text = item != null ? item.ItemName : "None";
                slotElement.Q<Label>("Amount").text = item != null ? $"{slotData.Entry.Amount}/{item.StackLimit}" : "-/-";

                var style = slotElement.Q<Button>("Icon").style.backgroundImage.value;
                style.sprite = item != null ? item.ItemIcon : null;
                slotElement.Q<Button>("Icon").style.backgroundImage = style;

                slots.Add(slotElement);
            }
        }
    }
}