using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// I CURRENTLY HAVE MADE TEST PREFABS FOR THE PLAYER, WALL, AND BOXES TO HELP SHOWCASE THE CURRENT SETUP.

public class GridMovement : MonoBehaviour
{
    // time the movement takes (can change if we want)
    [SerializeField] private float moveDuration = 0.1f;
    // grid size, change if needed
    [SerializeField] private float gridSize = 1f;
    private bool isMoving = false;

    void Update()
    {
        // this is the player movement
        if (!isMoving)
        {
            // set up currently for keyboard only, can add controller if we want
            var keyboard = Keyboard.current;

            // currently set up to only use WASD but can easily add arrow keys if we want
            if (keyboard.wKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.up));
            }
            else if (keyboard.sKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (keyboard.aKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.left));
            }
            else if (keyboard.dKey.wasPressedThisFrame)
            {
                StartCoroutine(Move(Vector2.right));
            }
        }
    }

    // this is for the actual grid movement
    private IEnumerator Move(Vector2 direction)
    {

        // check for current location and destination
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        // checks if player is open to move or if something is in the way
        // ENSURE PLAYER AND OBJECTS HAVE COLLIDERS
        Collider2D hitCollider = Physics2D.OverlapCircle(endPosition, 0.2f);
        if (hitCollider != null)
        {
            PushableBox box = hitCollider.GetComponent<PushableBox>();
            // check if it is a box or a wall, and if it is possible to even be moved
            if (box != null)
            {
                bool succPush = box.TryPush(direction);
                if (!succPush)
                {
                    yield break;
                }
            }
            else
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