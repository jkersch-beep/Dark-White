using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Make sure to add all new scenes to the scene list
    // Do this by going to File -> Build Profiles -> Open Scene List -> Add Open Scenes
    public string sceneToLoad;

    //Set the 'scene to load' in the script after attatching it to the TP
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // this is to save the position before the new scene is loaded
            PositionSaver[] savers = FindObjectsByType<PositionSaver>(FindObjectsSortMode.None);
            foreach (PositionSaver saver in savers)
            {
                saver.SavePosition();
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
