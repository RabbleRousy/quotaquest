using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] possibleItems;
    public Transform dropAreaContent;

    public void SpawnRandomItems(int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = Random.Range(0, possibleItems.Length);
            Instantiate(possibleItems[randomIndex], dropAreaContent);
        }
    }
}
