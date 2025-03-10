using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventorySlots;
    public GameObject itemPrefab;

    public void AddItem(string itemName, int itemValue)
    {
        GameObject newItem = Instantiate(itemPrefab);
        newItem.GetComponent<ItemObject>().Initialize(itemName, itemValue); 
        // Füge das Item zu einem freien Slot hinzu
    }

    public void RemoveItem(GameObject item)
    {
        inventorySlots.Remove(item);
        Destroy(item);
    }
}
