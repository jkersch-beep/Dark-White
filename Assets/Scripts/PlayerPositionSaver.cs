using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//I'll clean this up later. Currently programmed like its combined with the box script.

public class PlayerPositionSaver : MonoBehaviour
{
    //Generate the objects id
    //Keep everything in its own seperate hierarchy so that it will always work safely
    public string ObjectID => $"{gameObject.name}_{transform.GetSiblingIndex()}";

    // set the id based on the object name and the order in the hierarchy
    private string GetKey(string axis)
    {
        return $"{SceneManager.GetActiveScene().name}_{ObjectID}_{axis}";
    }

    // to load the save position. player is in start so that it loads last.
    private void Awake()
    {
        if (CompareTag("Player"))
        {
            LoadPosition();
        }
    }

    // call to save the positions to the player preferences
    public void SavePosition()
    {
        PlayerPrefs.SetFloat(GetKey("X"), transform.position.x);
        PlayerPrefs.SetFloat(GetKey("Y"), transform.position.y);
        PlayerPrefs.SetFloat(GetKey("Z"), transform.position.z);
        PlayerPrefs.Save();
    }

    // call to load the saved positions
    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey(GetKey("X")))
        {
            float x = PlayerPrefs.GetFloat(GetKey("X"));
            float y = PlayerPrefs.GetFloat(GetKey("Y"));
            float z = PlayerPrefs.GetFloat(GetKey("Z"));
            Vector3 savedPosition = new Vector3(x, y, z);

            if (CompareTag("Player"))
            {
                savedPosition = OffsetPosition(savedPosition);
            }
            transform.position = savedPosition;
        }
    }

    // clear the saved positions, currently only here for when the reset button is pressed.
    public void ClearSaves()
    {
        PlayerPrefs.DeleteKey(GetKey("X"));
        PlayerPrefs.DeleteKey(GetKey("Y"));
        PlayerPrefs.DeleteKey(GetKey("Z"));

        PushableBox box = GetComponent<PushableBox>();
        if (box != null)
        {
            box.enabled = true;
        }

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = false;
    }

    // Offset players position on revisiting the scene.
    private Vector3 OffsetPosition(Vector3 target)
    {
        float gridSize = 1f;

        if (IsBlocked(target))
        {
            Vector2[] directions = {Vector2.down, Vector2.left, Vector2.right, Vector2.up};
            foreach (Vector2 d in directions)
            {
                Vector2 toPos = (Vector2)target + (d * gridSize);
                if (!IsBlocked(toPos))
                {
                    return toPos;
                }
            }
        }
        return target;
    }

    private bool IsBlocked(Vector2 posCheck)
    {
        Collider2D hit = Physics2D.OverlapCircle(posCheck, 0.2f);

        if (hit != null)
        {
            return true;
        }
        return false;
    }
}
