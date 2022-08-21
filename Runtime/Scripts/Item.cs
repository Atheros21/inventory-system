using UnityEngine;

namespace ATH.InventorySystem
{
    /// <summary>
    /// Base scriptable object for an item.
    /// Derrive from this class to make game specific items
    /// </summary>
    public abstract class Item : ScriptableObject
    {
        [SerializeField] protected string _itemName;
        [SerializeField] protected string _itemDescription;
        [SerializeField] protected int _stackLimit;
        [SerializeField] protected Sprite _itemIcon;

        public string ItemName => _itemName;
        public string ItemDescription => _itemDescription;
        public int StackLimit => _stackLimit;
        public Sprite ItemIcon => _itemIcon;
    }
}
