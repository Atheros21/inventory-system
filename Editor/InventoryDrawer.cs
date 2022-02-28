using UnityEngine.UIElements;
using UnityEditor;

namespace ATH.InventorySystem
{
    public class InventoryDrawer
    {
        private const string kInventoryDrawerPath = "Assets/_Project/Scripts/InventorySystem/Editor/UXML/General/InventoryDrawer.uxml";
        private const string kInventorySlotDrawerPath = "Assets/_Project/Scripts/InventorySystem/Editor/UXML/General/InventorySlotDrawer.uxml";

        private VisualTreeAsset _inventoryVisualTreeAsset;
        private VisualTreeAsset _inventorySlotTreeAsset;
        private Inventory _inventory;
        private VisualElement _root;
        private bool _usingExistingElement;

        public VisualElement Root => _root;

        public InventoryDrawer(Inventory inventory)
        {
            _inventoryVisualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(kInventoryDrawerPath);
            _inventorySlotTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(kInventorySlotDrawerPath);
            _inventory = inventory;
            CreateNewRoot();

            if (inventory != null)
                _inventory.OnItemUpdate.AddListener(CreateNewRoot);
        }

        public InventoryDrawer(Inventory inventory, VisualElement extisingDrawer)
        {
            _inventorySlotTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(kInventorySlotDrawerPath);
            _inventory = inventory;
            _root = extisingDrawer;
            _usingExistingElement = true;
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
                var item = slotData.Item;

                slotElement.Q<Label>("Name").text = item != null ? item.ItemName : "None";
                slotElement.Q<Label>("Amount").text = item != null ? $"{slotData.Amount}/{item.StackLimit}" : "-/-";

                var style = slotElement.Q<Button>("Icon").style.backgroundImage.value;
                style.sprite = item != null ? item.ItemIcon : null;
                slotElement.Q<Button>("Icon").style.backgroundImage = style;

                slots.Add(slotElement);
            }
        }
    }
}