using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    // Set this flag to true when the dialogue panel is active
    private bool inDialogue { get; set; }

    public bool IsInDialogue()
    {
        return inDialogue;
    }

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }

    public void StartDialogue()
    {
        inDialogue = true; // Set the InDialogue flag to true when the dialogue starts
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        inDialogue = false; // Set the InDialogue flag to false when the dialogue ends

        // You can remove the attempt to access npcAnimator here, as it's managed in NPCInteraction script

        gameObject.SetActive(false);
    }
}
