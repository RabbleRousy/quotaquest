using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public GameObject lootPrefab;
    public Transform lootContainer;

    public void GenerateLoot(string eventName)
    {
        // Generiere zuf�lligen Loot basierend auf dem Event
        GameObject loot = Instantiate(lootPrefab, lootContainer);
        // F�ge Effekte hinzu
    }
}