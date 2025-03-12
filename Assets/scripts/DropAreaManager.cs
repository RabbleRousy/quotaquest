using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropAreaManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private Transform[] itemSpawnPoints;

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

    public void SpawnItems(EventData e)
    {
        Transform canvas = FindFirstObjectByType<Canvas>().transform;
        for (int i = 0; i < e.itemAmount; i++)
        {
            Item item = Instantiate(e.possibleItems[Random.Range(0, e.possibleItems.Length)], canvas);
            item.transform.position = itemSpawnPoints[i].position;
            item.GetComponent<DragDropItem>().InDropArea = true;
            AddItem(item);
        }
    }
}