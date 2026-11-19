using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // These are added so that Hole.cs can check info.
    public bool IsMoving => isMoving;
    public bool HasConnectedNeighbors()
    {
        List<PushableBox> group = new List<PushableBox>();
        CheckAdjacentBoxes(group);
        return group.Count > 1;
    }

    public bool TryPush(Vector2 direction)
    {
        // check if the box is already moving (ensuring a box isnt accidentally pushed twice by the player)
        if (isMoving)
        {
            return false;
        }

        // Put all connected boxes into a list
        List<PushableBox> group = new List<PushableBox>();
        CheckAdjacentBoxes(group);

        // Check box paths before movement
        foreach (var box in group)
        {
            Vector2 targetPosition = (Vector2)box.transform.position + (direction * gridSize);
            Collider2D collider = Physics2D.OverlapCircle(targetPosition, 0.2f, whatStopsBox);
            if (collider != null)
            {
                PushableBox otherBox = collider.GetComponent<PushableBox>();
                if (otherBox != null && group.Contains(otherBox))
                 {
                    continue;
                }
                return false;
            }
        }

        // the box moves
        StartCoroutine(Move(group, direction));
        return true;
    }

    // this is for the actual grid movement
    private IEnumerator Move(List<PushableBox> group, Vector2 direction)
    {
        // Set all boxes to moving
        foreach (var box in group)
        {
            box.isMoving = true;
        }

        // Get the box positions and save them
        Dictionary<PushableBox, Vector3> startPositions = new Dictionary<PushableBox, Vector3>();
        Dictionary<PushableBox, Vector3> endPositions = new Dictionary<PushableBox, Vector3>();

        foreach (var box in group)
        {
            startPositions[box] = box.transform.position;
            endPositions[box] = startPositions[box] + (Vector3)(direction * gridSize);
        }

        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / moveDuration;

            foreach (var box in group)
            {
                box.transform.position = Vector3.Lerp(startPositions[box], endPositions[box], percent);
            }
            yield return null;
        }

        foreach (var box in group)
        {
            box.transform.position = endPositions[box];
            box.isMoving = false;
        }
    }

    // Box list management (i realize the name i gave this doesnt make sense anymore, im just too lazy to go back and change it since its working ngl)
    private void CheckAdjacentBoxes(List<PushableBox> groupToFill)
    {
        if (groupToFill.Contains(this))
        {
            return;
        }

        groupToFill.Add(this);


        Vector2[] directions = {Vector2.down, Vector2.left, Vector2.right, Vector2.up};
        foreach (Vector2 d in directions)
        {
            Vector2 toPos = (Vector2)transform.position + (d * gridSize);
            Collider2D collider = Physics2D.OverlapCircle(toPos, 0.2f, whatStopsBox);
            if (collider != null && !collider.isTrigger)
            {
                if (collider.CompareTag("Box"))
                {
                    PushableBox otherBox = collider.GetComponent<PushableBox>();
                    if (otherBox != null)
                    {
                        otherBox.CheckAdjacentBoxes(groupToFill);
                    }
                }
            }
        }
    }
}