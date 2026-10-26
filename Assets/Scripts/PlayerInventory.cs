using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public static class PlayerInventory
{
    // Save all items collected. Can be reset by pressing R in game.
    public static void AddItem(string itemID)
    {
        PlayerPrefs.SetInt("Item_" + itemID, 1);
        PlayerPrefs.Save();
    }
    // Checks for the saved item when it is required for an object.
    public static bool HasItem(string itemID)
    {
        return PlayerPrefs.GetInt("Item_" + itemID, 0) == 1;
    }
}
