using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = .1f;
    private int index;
    public Image portraitImageUI;
    public Image fullScreenUI;
    public Sprite[] portraits;
    public bool[] fullScreen;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.eKey.wasPressedThisFrame)
        {

            bool isFull = (fullScreen != null && index < fullScreen.Length && fullScreen[index]);
            if (textComponent.text == lines[index] || isFull)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        bool isFull = (fullScreen != null && index < fullScreen.Length && fullScreen[index]);
        fullScreenUI.enabled = isFull;
        fullScreenUI.sprite = portraits[index];
        portraitImageUI.sprite = portraits[index];
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            bool isFull = (fullScreen != null && index < fullScreen.Length && fullScreen[index]);
            fullScreenUI.enabled = isFull;
            if (index < portraits.Length)
            {
                fullScreenUI.sprite = portraits[index];
                portraitImageUI.sprite = portraits[index];
            }
            StartCoroutine(TypeLine());
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
