using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// ADDING THESE HERE TEMPORARILY:
// WILL MAKE A SCRIPT FOR BOX MOVEMENT
// WILL MAKE A SEPERATE SCRIPT FOR CAMERA MOVEMENT

public class GridMovement : MonoBehaviour
{
    // time the movement takes (can change if we want)
    [SerializeField] private float moveDuration = 0.1f;
    // grid size, change if needed
    [SerializeField] private float gridSize = 1f;
    // default to not moving
    private bool isMoving = false;

    // update, checks per each frame
    void Update()
    {
        // checks if is moving then determines the input
        if (!isMoving)
        {
            // set up currently for keyboard only, can add controller if we want
            var keyboard = Keyboard.current;

            // checking what key was pressed and then moving in the direction
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

    // movement between the grid
    private IEnumerator Move(Vector2 direction)
    {
        // set that we are moving
        isMoving = true;

        // check for current location and destination
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        // move in direction with desired time
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        // ensure we moved to the right spot
        transform.position = endPosition;

        // no longer moving, changing back
        isMoving = false;
    }
}