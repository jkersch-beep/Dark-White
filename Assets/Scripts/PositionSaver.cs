using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionSaver : MonoBehaviour
{
    //Generate the objects id
    //Keep everything in its own seperate hierarchy so that it will always work safely
    public string ObjectID => $"{gameObject.name}_{transform.GetSiblingIndex()}";

    // set the id based on the object name and the order in the hierarchy
    private string GetKey(string axis)
    {
        return $"{SceneManager.GetActiveScene().name}_{ObjectID}_{axis}";
    }

    // to load the save position
    private void Awake()
    {
        LoadPosition();
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
            transform.position = new Vector3(x, y, z);
        }
    }

    // clear the saved positions, currently only here for when the reset button is pressed.
    public void ClearSaves()
    {
        PlayerPrefs.DeleteKey(GetKey("X"));
        PlayerPrefs.DeleteKey(GetKey("Y"));
        PlayerPrefs.DeleteKey(GetKey("Z"));
    }
}
