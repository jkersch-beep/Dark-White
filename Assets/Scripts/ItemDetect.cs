using System.Collections;
using UnityEngine;

public class ItemDetect : MonoBehaviour
{

    // Attatch the TP to the activateTP thing in the script attatched to the prefab
    // Change the tag in the script attatched to the prefab if it is not a box
    [SerializeField] private string detectItem = "Box";
    [SerializeField] private GameObject activateTP;

    // TP starts hidden
    private void Start()
    {
        activateTP.SetActive(false);
    }
    
    // TP shows after the box collides with the yellow tile (or whatever we attatch this script to)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == detectItem && activateTP != null)
        {
            activateTP.SetActive(true);
        }
    }

    // TP is gone if box is moved off
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == detectItem && activateTP != null)
        {
            activateTP.SetActive(false);
        }
    }
}
