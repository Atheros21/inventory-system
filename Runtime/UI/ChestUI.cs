using UnityEngine;
using UnityEngine.UIElements;

namespace ATH.InventorySystem
{
    public class ChestUI : MonoBehaviour
    {
        [SerializeField] private UIWrapper _uiWrapper;
        [SerializeField] private UIDocument _uiDocument;

        private VisualElement _cheatsPanelRoot;
        private VisualElement _header;
        private bool _isDragged;

        private void Awake()
        {
            _cheatsPanelRoot = _uiDocument.rootVisualElement.Q<VisualElement>("ChestUI");

            _cheatsPanelRoot.Q<Button>("CloseChestPanel").clicked += CloseChestUI;
            _header = _cheatsPanelRoot.Q<VisualElement>("Header");
            _header.RegisterCallback<MouseDownEvent>(OnMouseClick);
            _header.RegisterCallback<MouseUpEvent>(OnMouseUp);
            _header.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            _header.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);

            _uiWrapper.OnChestOpen += OpenChestUI;
            _uiWrapper.OnChestClose += CloseChestUI;
        }

        private void OnDestroy()
        {
            _uiWrapper.OnChestOpen -= OpenChestUI;
            _uiWrapper.OnChestClose -= CloseChestUI;
        }

        private void OnMouseUp(MouseUpEvent mouseUpEvent)
        {
            Debug.Log("Release");
            _isDragged = false;
        }

        private void OnMouseClick(MouseDownEvent mouseDownEvent)
        {
            Debug.Log("Press");
            _isDragged = true;
        }

        private void OnMouseOutEvent(MouseOutEvent mouseMoveEvent)
        {
            Debug.Log("Out");
            _isDragged = false;
        }

        private void OnMouseMoveEvent(MouseMoveEvent mouseMoveEvent)
        {
            Debug.Log("Move");
            if (!_isDragged) return;

            _cheatsPanelRoot.transform.position += new Vector3(mouseMoveEvent.mouseDelta.x, mouseMoveEvent.mouseDelta.y);
        }

        private void OpenChestUI(Inventory inventory)
        {
            _cheatsPanelRoot.style.display = DisplayStyle.Flex;
        }

        private void CloseChestUI()
        {
            _cheatsPanelRoot.style.display = DisplayStyle.None;
        }
    }
}