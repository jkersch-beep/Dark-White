using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    [SerializeField] private int boxesRequired = 2;
    private int boxesAdded = 0;
    public bool isFilled = false;
    public bool IsFilled => isFilled;

    public string holeCountSaver => $"{SceneManager.GetActiveScene().name}_{gameObject.name}_{transform.GetSiblingIndex()}";

    private void Awake()
    {
        LoadHoleCount();
    }
    private void Update()
    {
        if (isFilled) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var hit in hits)
        {
            PushableBox box = hit.GetComponent<PushableBox>();
            if (box != null)
            {
                FillHole(box);
                break;
            }
        }
    }

    private void FillHole(PushableBox box)
    {
        boxesAdded++;
        box.transform.position = transform.position;

        if (boxesAdded == boxesRequired)
        {
            isFilled = true;
            Collider2D boxCollider = box.GetComponent<Collider2D>();
            boxCollider.isTrigger = true;
            box.enabled = false;
            gameObject.SetActive(false);
        } else
        {
            PositionSaver saver = box.GetComponent<PositionSaver>();
            saver.SavePosition();
            box.gameObject.SetActive(false);
        }
    }

    private void LoadHoleCount()
    {
        if (PlayerPrefs.HasKey(holeCountSaver))
        {
            boxesAdded = PlayerPrefs.GetInt(holeCountSaver);
        }
    }

    public void ClearHoleCount()
    {
        PlayerPrefs.DeleteKey(holeCountSaver);
        boxesAdded = 0;
        isFilled = false;
    }
}
