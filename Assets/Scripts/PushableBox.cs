using System.Collections;
using UnityEngine;

// partially reuses stuff from GridMovement.cs

public class PushableBox : MonoBehaviour
{
    // time the movement takes (can change if we want)
    // keep consistent with GridMovement.cs
    [SerializeField] private float moveDuration = 0.1f;
    // grid size, change if needed
    // keep consistent with GridMovement.cs
    [SerializeField] private float gridSize = 1f;
    private bool isMoving = false;
    // THIS IS FOR LAYERS. CURRENT LOGIC IS THAT WALL AND BOX ARE IN THE SAME LAYER, CAN ADJUST IF WE NEED.
    [SerializeField] private LayerMask whatStopsBox;

    public bool TryPush(Vector2 direction)
    {
        // check if the box is already moving (ensuring a box isnt accidentally pushed twice by the player)
        if (isMoving)
        {
            return false;
        }

        // finding the destinatin of the box/wall
        Vector2 targetPosition = (Vector2)transform.position + (direction * gridSize);
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, whatStopsBox))
        {
            return false;
        }
        StartCoroutine(Move(direction));
        return true;
    }

    // this is for the actual grid movement
    private IEnumerator Move(Vector2 direction)
    {
        // keep before actual movement
        isMoving = true;

        // check for current location and destination
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

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