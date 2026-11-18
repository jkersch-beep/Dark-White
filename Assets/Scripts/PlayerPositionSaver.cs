using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionSaver : MonoBehaviour
{
    private void Awake()
    {
        if (CompareTag("Player"))
        {
            LoadPosition();
        }
    }

    // call to load the saved positions
    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey("TargetSpawnX"))
        {
            float x = PlayerPrefs.GetFloat("TargetSpawnX");
            float y = PlayerPrefs.GetFloat("TargetSpawnY");
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    // clear the saved positions, currently only here for when the reset button is pressed.
    public void ClearSaves()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.DeleteKey("TargetSpawnX");
        PlayerPrefs.DeleteKey("TargetSpawnY");

        PushableBox box = GetComponent<PushableBox>();
        if (box != null)
        {
            box.enabled = true;
        }

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = false;
    }
}