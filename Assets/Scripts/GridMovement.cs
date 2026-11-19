using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GridMovement : MonoBehaviour
{
    // time the movement takes (can change if we want)
    [SerializeField] private float moveDuration = 0.1f;
    // grid size, change if needed
    [SerializeField] private float gridSize = 1f;
    private bool isMoving = false;

    void Update()
    {
        var keyboard = Keyboard.current;
        // this is the player movement
        if (!isMoving)
        {
            // set up currently for keyboard only, can add controller if we want
            if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.up));
            }
            else if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.left));
            }
            else if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.right));
            }
        }

        // keybind to reset the scene, we can add additional keybinds here if we want
        if (keyboard.leftCtrlKey.wasPressedThisFrame)
        {
            // clears the saved positions before the scene 
            PositionSaver[] savers = FindObjectsByType<PositionSaver>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (PositionSaver saver in savers)
            {
                saver.gameObject.SetActive(true);
                saver.ClearSaves();
            }

            Hole[] holes = FindObjectsByType<Hole>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Hole hole in holes)
            {
                hole.gameObject.SetActive(true);
                hole.ClearHoleCount();
            }
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        // keybind for interactions
        if (keyboard.eKey.wasPressedThisFrame)
        {
            LockedDoor.TryInteractingWithObject();
            ItemSceneLoad.TryInteractingWithObject();
        }

        //use these to clear player prefs (specifically for keys)
        if (keyboard.rKey.wasPressedThisFrame)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    // this is for the actual grid movement
    private IEnumerator Move(Vector2 direction)
    {

        // check for current location and destination
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        // checks if player is open to move or if something is in the way
        // ENSURE PLAYER AND OBJECTS HAVE COLLIDERS.
        Collider2D[] hits = Physics2D.OverlapCircleAll(endPosition, 0.2f);

        Hole targetHole = null;
        PushableBox targetBox = null;
        bool hasSolidObstacle = false;

        foreach (var hit in hits)
        {
            if (hit == null)
            {
                continue;
            }

            Hole hole = hit.GetComponent<Hole>();
            if (hole != null && !hole.IsFilled)
            {
                targetHole = hole;
            }

            PushableBox box = hit.GetComponent<PushableBox>();
            if (box != null)
            {
                targetBox = box;
            }
            else if (!hit.isTrigger && targetHole == null)
            {
                hasSolidObstacle = true;
            }

        }

        if (hasSolidObstacle)
        {
            yield break;
        }

        if (targetHole != null)
        {
            if (targetBox == null)
            {
                yield break;
            }
            else
            {
                bool succPush = targetBox.TryPush(direction);
                yield break;
            }
        }

        if (targetBox != null)
        {
            bool succPush = targetBox.TryPush(direction);
            if (!succPush)
            {
                yield break;
            }
        }

        // make sure to keep this AFTER the wall/box check
        isMoving = true;

        // the actual movement happening
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        // here to ensure movement was to the right spot
        transform.position = endPosition;

        // keep at the end
        isMoving = false;
    }
}