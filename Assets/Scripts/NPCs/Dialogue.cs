using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public AudioClip[] audioClips; 
    public float textSpeed;

    private int index;
    private bool inDialogue { get; set; }
    private AudioSource audioSource;

    public bool IsInDialogue()
    {
        return inDialogue;
    }

    private void Start()
    {
        textComponent.text = string.Empty;
        audioSource = GetComponent<AudioSource>();
        StartDialogue();
    }

    private void Update()
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
        inDialogue = true;
        index = 0;
        StartCoroutine(TypeLine());
        PlayAudio(); 
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        StopAudio(); 
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            PlayAudio(); 
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        inDialogue = false;
        gameObject.SetActive(false);
    }

    private void PlayAudio()
    {
        if (audioClips != null && audioClips.Length > index)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
