using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace ATH.InventorySystem
{
    public class InventoryDrawer
    {
        private VisualTreeAsset _inventoryVisualTreeAsset;
        private VisualTreeAsset _inventorySlotTreeAsset;
        private Inventory _inventory;
        private VisualElement _root;
        private VisualElement _nullContainer;
        private VisualElement _validContainer;
        private Action<Inventory> _newInventoryCreated;
        private UnityEngine.Object _target;
        private SerializedObject _serializedObject;

        public VisualElement Root => _root;

        public InventoryDrawer(Inventory inventory, Action<Inventory> newInventoryCreated, UnityEngine.Object target, SerializedObject serializedObject)
        {
            _inventoryVisualTreeAsset = UXMLSet.GetUxmlSetInstance().Inventory;
            _inventorySlotTreeAsset = UXMLSet.GetUxmlSetInstance().InventorySlot;
            _inventory = inventory;
            _newInventoryCreated = newInventoryCreated;
            _target = target;
            _serializedObject = serializedObject;
            CreateNewRoot();
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
            _validContainer = root.Q<VisualElement>("ValidInventory");
            _nullContainer = root.Q<VisualElement>("NullInventory");
          
            if (_inventory == null)
            {
                _nullContainer.style.display = DisplayStyle.Flex;
                _validContainer.style.display = DisplayStyle.None;
                var createBtn = _nullContainer.Q<Button>("CreateInventory");
                createBtn.clicked -= CreateBtn;
                createBtn.clicked += CreateBtn;
                return;
            }

            _nullContainer.style.display = DisplayStyle.None;
            _validContainer.style.display = DisplayStyle.Flex;

            var goldInput = _validContainer.Q<IntegerField>("Gold");
            goldInput.SetValueWithoutNotify(_inventory.Gold);
            goldInput.UnregisterCallback<ChangeEvent<int>>(ChangeGold);
            goldInput.RegisterCallback<ChangeEvent<int>>(ChangeGold);

            return;

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

        private void CreateBtn()
        {
            if (_nullContainer == null) return;

            var inventory = new Inventory(_nullContainer.Q<IntegerField>("Width").value, _nullContainer.Q<IntegerField>("Height").value);
            _newInventoryCreated?.Invoke(inventory);
            _inventory = inventory;
            UpdateVisuals(_root);
            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private void ChangeGold(ChangeEvent<int> args)
        {
            _inventory.SetGold(args.newValue);
            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}