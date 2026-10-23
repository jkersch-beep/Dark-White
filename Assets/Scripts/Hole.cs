using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{

    // CURRENT KNOWN ISSUE: NEED TO ADJUST HOLE TO DEAL WITH BOXES STUCK TOGETHER

    // Count boxes, check for if a hole is filled, etc.
    [SerializeField] private int boxesRequired = 2;
    private int boxesAdded = 0;
    public bool isFilled = false;
    public bool IsFilled => isFilled;

    private void Update()
    {
        if (isFilled) return;

        //Check if anything overlaps the hole
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var hit in hits)
        {
            PushableBox box = hit.GetComponent<PushableBox>();
            if (box != null && box.enabled)
            {
                FillHole(box);
                break;
            }
        }
    }

    private void FillHole(PushableBox box)
    {
        // When a box is pushed into a hole, increase count, change box position below hole, make hole passable.
        boxesAdded++;
        box.StopAllCoroutines();
        Vector3 fallPosition = transform.position;
        fallPosition.z += 1f;
        box.transform.position = fallPosition;
        box.enabled = false;
        Collider2D boxCollider = box.GetComponent<Collider2D>();
        boxCollider.isTrigger = true;
        //permanently fill hole if enough boxes have been added
        if (boxesAdded >= boxesRequired)
        {
            isFilled = true;
            gameObject.SetActive(false);
        }
    }

    // Reset hole
    public void ClearHoleCount()
    {
        boxesAdded = 0;
        isFilled = false;

        Collider2D holeCollider = GetComponent<Collider2D>();
        holeCollider.isTrigger = false;
    }
}
