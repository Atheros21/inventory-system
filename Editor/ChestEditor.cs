using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ATH.InventorySystem
{
    [CustomEditor(typeof(Chest))]
    public class ChestEditor : Editor
    {
        private Chest _chest;
        private VisualElement _root;
        private VisualTreeAsset _rootTreeAsset;
        private InventoryDrawer _inventoryDrawer;
        private IntegerField _goldField;
        private ObjectField _itemAddField;
        private ObjectField _itemRemoveField;
        private VisualElement _inventoryDrawerRoot;

        private void OnEnable()
        {
            _chest = (Chest)target;
            _root = new VisualElement();
            _inventoryDrawer = new InventoryDrawer(_chest.Inventory);
            _rootTreeAsset = UXMLSet.GetUxmlSetInstance().Chest;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = _root;
            root.Clear();
            _rootTreeAsset.CloneTree(root);
            root.Q<Button>("reset").clicked += () =>
            {
                _chest.ResetInventory();
                _inventoryDrawer = new InventoryDrawer(_chest.Inventory);
                _inventoryDrawer.UpdateVisuals(_inventoryDrawerRoot);
                EditorUtility.SetDirty(_chest);
            };

            root.Q<Button>("add-item").clicked += AddItem;
            root.Q<Button>("remove-item").clicked += RemoveItem;
            root.Q<Button>("change-gold").clicked += ChagneGold;

            _itemAddField = root.Q<ObjectField>("item-add-field");
            _itemRemoveField = root.Q<ObjectField>("item-remove-field");
            _goldField = root.Q<IntegerField>("gold-value");
            _inventoryDrawerRoot = root.Q<VisualElement>("inventory-drawer");

            _inventoryDrawer.UpdateVisuals(_inventoryDrawerRoot);

            return root;
        }

        private void ChagneGold()
        {
            if (_chest.Inventory == null)
            {
                Debug.Log("Null inventory");
                return;
            }

            _chest.Inventory.SetGold(_goldField.value);
            _inventoryDrawer.UpdateVisuals(_inventoryDrawerRoot);
            EditorUtility.SetDirty(_chest);
        }

        private void AddItem()
        {
            if (_chest.Inventory == null)
            {
                Debug.Log("Null inventory");
                return;
            }

            if (_itemAddField.value == null)
            {
                Debug.Log("Selected item is null");
                return;
            }

            var item = _itemAddField.value as Item;

            if (item != null && !_chest.Inventory.CanAddItem(item, 1))
            {
                Debug.Log("Can't add item");
                return;
            }

            _chest.Inventory.AddItem(item, 1);
            _inventoryDrawer.UpdateVisuals(_inventoryDrawerRoot);
            EditorUtility.SetDirty(_chest);
        }

        private void RemoveItem()
        {
            if (_chest.Inventory == null)
            {
                Debug.Log("Null inventory");
                return;
            }

            if (_itemRemoveField.value == null)
            {
                Debug.Log("Selected item is null");
                return;
            }

            var item = _itemRemoveField.value as Item;

            if (item != null && !_chest.Inventory.CanRemoveItem(item, 1))
            {
                Debug.Log("Can't remove item");
                return;
            }

            _chest.Inventory.RemoveItem(item, 1);
            _inventoryDrawer.UpdateVisuals(_inventoryDrawerRoot);
            EditorUtility.SetDirty(_chest);
        }
    }
}