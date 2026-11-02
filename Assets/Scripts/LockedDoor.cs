using System.Collections;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    // Fields that need to be set for each new door / key.
    [SerializeField] private string pathID = "TestingDoor";
    // MATCH THIS TO THE ITEM THAT IS PICKED UP AND REQUIRED FOR THE DOOR
    [SerializeField] private string itemIDRequired = "TestingKey";
    [SerializeField] private GameObject objectBlocking;
    // Also make sure to set this script on the interaction collider.
    [SerializeField] private Collider2D interactionCollider;
    // Replacement object is not required, just optional.
    [SerializeField] private GameObject replacementObject;
    [SerializeField] private GameObject InteractUI;
    [SerializeField] private GameObject LockedMessage;
    private bool pathUnlocked = false;
    private static LockedDoor objectInRange;

    // Check if the door was already unlocked when entering scene. Unlock door if so.
    void Start()
    {
        InteractUI.SetActive(false);
        LockedMessage.SetActive(false);
        if (PlayerPrefs.GetInt(GetObjectSave(), 0) == 1)
        {
            UnlockOnStart();
        }
        else
        {
            if (replacementObject != null)
            {
                replacementObject.SetActive(false);
            }
        }
    }

    // Detect when the player enters the interaction collider. Pop up with the message to unlock and allow unlocking.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            objectInRange = this;

            if (!pathUnlocked)
            {
                if (PlayerInventory.HasItem(itemIDRequired))
                {
                    InteractUI.SetActive(true);
                }
                else
                {
                    LockedMessage.SetActive(true);
                }
            }
        }
    }

    // Detect if the player leaves the collision area.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectInRange == this)
            {
                objectInRange = null;
                InteractUI.SetActive(false);
                LockedMessage.SetActive(false);
            }
        }
    }

    // Check if you are in range to unlock the door when called.
    public static void TryInteractingWithObject()
    {
        if (objectInRange != null)
        {
            objectInRange.TryUnlock();
        }
    }

    // Check if the player has the key and can unlock the door. Save that the door is unlocked.
    private void TryUnlock()
    {
        if (pathUnlocked) return;
        if (PlayerInventory.HasItem(itemIDRequired))
        {
            pathUnlocked = true;
            PlayerPrefs.SetInt(GetObjectSave(), 1);
            PlayerPrefs.Save();
            UnlockOnStart();
        }
    }

    // Unlock the door.
    private void UnlockOnStart()
    {
        pathUnlocked = true;
        interactionCollider.enabled = false;
        objectBlocking.SetActive(false);
        if (replacementObject != null)
        {
            replacementObject.SetActive(true);
        }
    }

    //Generates the door id.
    private string GetObjectSave()
    {
        return "DoorUnlocked" + pathID;
    }
}