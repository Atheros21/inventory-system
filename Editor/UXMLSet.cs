using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ATH.InventorySystem
{
    [CreateAssetMenu]
    public class UXMLSet : ScriptableObject
    {
        public VisualTreeAsset Inventory;
        public VisualTreeAsset InventorySlot;

        public VisualTreeAsset Chest;

        public static UXMLSet GetUxmlSetInstance()
        {
            string[] guids = AssetDatabase.FindAssets("t:UXMLSet");
            if(guids.Length > 1)
            {
                Debug.LogError("Multiple UXML set found, check integrity of project");
            }
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<UXMLSet>(path);
        }
    } 
}