using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DropAreaManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    public void AddItem(Item item) => items.Add(item);
    public void RemoveItem(Item item) => items.Remove(item);

    private void OnDisable()
    {
        // Destroy all items that were left here
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();
    }
}