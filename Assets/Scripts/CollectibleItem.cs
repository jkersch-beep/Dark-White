using System.Collections;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    // PLEASE UPDATE THE ITEM ID TO MATCH WHATEVER ITEM IT IS ATTATCHED TO.
    // ALSO MAKE SURE EACH ITEM HAS A UNIQUE ID.
    [SerializeField] private string itemID = "TestingKey";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If the item has already been collected, then it will stay hidden on re entry
        if (PlayerPrefs.GetInt(GetItemSave(), 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for player, if so then make item dissapear, save it, and update inventory.
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(GetItemSave(), 1);
            PlayerPrefs.Save();
            PlayerInventory.AddItem(itemID);
            gameObject.SetActive(false);
        }
    }

    // Saving the item state
    private string GetItemSave()
    {
        return "Item_" + itemID;
    }
}
