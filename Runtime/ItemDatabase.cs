using System.Collections.Generic;
using UnityEngine;


namespace ATH.InventorySystem
{
    [CreateAssetMenu(fileName = "ItemDatabse", menuName = "Databases/Items")]
    public class ItemDatabase : ScriptableObject
    {
        public List<Item> items;
    }

}