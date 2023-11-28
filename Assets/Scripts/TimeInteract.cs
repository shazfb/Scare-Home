using System.Collections;
using UnityEngine;

public class TimeInteract : Interactable
{
    public TimeController timeController;
    public GameObject timeInteractionBox;
    public GameObject timeInteractText;
    public ScreenFade screenFade;

    public AudioClip doorbangSound;  
    private AudioSource audioSource;
    public AudioSource backgroundAudio;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = doorbangSound;
        audioSource.volume = 1.0f;
        
        if (backgroundAudio != null)
        {
            backgroundAudio.Stop();
        }
    }

    public override void OnFocus()
    {
        timeInteractText.SetActive(true);
    }

    public override void OnInteract()
    {
        StartCoroutine(InteractCoroutine());

    }

    private IEnumerator InteractCoroutine()
    {
  
        screenFade.StartFadeIn();
       
        

        yield return new WaitForSeconds(screenFade.fadeDuration);

        audioSource.Play();
       
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        timeController.ChangeTime(1f);
        
        timeInteractionBox.SetActive(false);
        
        timeInteractText.SetActive(false);

        StartBackgroundAudio();
    }

    public override void OnLoseFocus()
    {
        timeInteractText.SetActive(false);
    }

    public void StartBackgroundAudio()
    {
        if (backgroundAudio != null && !backgroundAudio.isPlaying)
        {
            backgroundAudio.Play();
        }
    }
}

