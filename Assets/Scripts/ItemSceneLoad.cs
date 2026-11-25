using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSceneLoad : MonoBehaviour
{
    // Fields that need to be set for each new door / key.
    [SerializeField] private string pathID = "TestingDoor1";
    // MATCH THIS TO THE ITEM THAT IS PICKED UP AND REQUIRED FOR THE DOOR
    [SerializeField] private string itemIDRequired = "TestingKey1";
    // Also make sure to set this script on the interaction collider.
    [SerializeField] private Collider2D interactionCollider;
    [SerializeField] private GameObject InteractUI;
    [SerializeField] private GameObject LockedMessage;
    [SerializeField] private Vector2 targetSpawnPos;
    private static ItemSceneLoad objectInRange;
    public string sceneToLoad;
    private bool RequiresKey => !string.IsNullOrWhiteSpace(itemIDRequired);

    private void Start()
    {
        InteractUI.SetActive(false);
        LockedMessage.SetActive(false);
    }

    // Detect when the player enters the interaction collider. Pop up with the message to unlock and allow unlocking.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            objectInRange = this;

            if (!RequiresKey || PlayerInventory.HasItem(itemIDRequired))
            {
                InteractUI.SetActive(true);
            }
            else
            {
                LockedMessage.SetActive(true);
            }
        }
    }

    // Detect if the player leaves the collision area.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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

    // Check if the player has the key and can unlock the door. TP after if so.
    private void TryUnlock()
    {
        if (!RequiresKey || PlayerInventory.HasItem(itemIDRequired))
        {
            // this is to save the position before the new scene is loaded
            PositionSaver[] savers = FindObjectsByType<PositionSaver>(FindObjectsSortMode.None);
            foreach (PositionSaver saver in savers)
            {
                saver.SavePosition();
            }

            PlayerPrefs.SetFloat("TargetSpawnX", targetSpawnPos.x);
            PlayerPrefs.SetFloat("TargetSpawnY", targetSpawnPos.y);
            PlayerPrefs.Save();

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}