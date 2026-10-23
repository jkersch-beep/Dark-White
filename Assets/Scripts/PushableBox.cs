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
    // check if the box has already stuck to another one
    private bool isStuck = false;

    public bool TryPush(Vector2 direction)
    {
        // check if the box is already moving (ensuring a box isnt accidentally pushed twice by the player)
        if (isMoving)
        {
            return false;
        }

        // Create a leader for connected boxes so that they move together
        PushableBox leader = FindLeader();
        if (leader != this)
        {
            return leader.TryPush(direction);
        }

        // Each box has an individual path check then determines movement.
        PushableBox[] groupBoxes = GetComponentsInChildren<PushableBox>();
        foreach (var box in groupBoxes)
        {
            Vector2 targetPosition = (Vector2)box.transform.position + (direction * gridSize);
            Collider2D collider = Physics2D.OverlapCircle(targetPosition, 0.2f, whatStopsBox);

            if (collider != null && !collider.isTrigger)
            {
                PushableBox otherBox = collider.GetComponent<PushableBox>();
                if (otherBox != null)
                {
                    // 1: checks if box is connected, 2: stick if not currently stuck, 3: stop movement.
                    if (otherBox.FindLeader() == this)
                    {
                        continue;
                    }

                    if (!isStuck)
                    {
                        StickToBox(otherBox);
                        return false;
                    }
                }
                return false;
            }
        }
        StartCoroutine(Move(direction));
        return true;
    }

    // this is for the actual grid movement
    private IEnumerator Move(Vector3 direction)
    {
        PushableBox[] groupBoxes = GetComponentsInChildren<PushableBox>();
        foreach (var box in groupBoxes)
        {
            box.isMoving = true;
        }

        // check for current location and destination
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + (direction * gridSize);

        // the actual movement happening
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        // here to ensure movement was to the right spot
        transform.position = endPosition;

        // keep at the end
        foreach (var box in groupBoxes)
        {
            box.isMoving = false;
        }
    }

    // Combine two boxes and mark as stuck
    private void StickToBox(PushableBox target)
    {
        this.isStuck = true;
        target.isStuck = true;

        PushableBox targetLeader = target.FindLeader();
        transform.SetParent(targetLeader.transform, true);
    }

    // Determine the leader of a box group
    private PushableBox FindLeader()
    {
        Transform current = transform;
        while (current.parent != null)
        {
            PushableBox leader = current.parent.GetComponent<PushableBox>();
            if (leader != null)
            {
                current = current.parent;
            } else
            {
                break;
            }
        }
        return current.GetComponent<PushableBox>();
    }
}