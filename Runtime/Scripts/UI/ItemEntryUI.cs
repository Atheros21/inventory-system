using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ATH.InventorySystem.UI
{
    public class ItemEntryUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private Image _itemIcon;

        private Canvas _canvasWhileDragging;
        private Canvas _canvas;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetParentCanvas(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void SetCanvasWhileDragging(Canvas canvas)
        {
            _canvasWhileDragging = canvas;
        }

        public void UpdateEntry(ItemEntry itemEntry)
        {
            var hasItem = itemEntry.Item != null && itemEntry.Amount > 0;
            _amount.gameObject.SetActive(hasItem);
            _itemIcon.gameObject.SetActive(hasItem);

            if (!hasItem) return;

            _amount.SetText(itemEntry.Amount.ToString());
            _itemIcon.sprite = itemEntry.Item.ItemIcon;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

    }
}
