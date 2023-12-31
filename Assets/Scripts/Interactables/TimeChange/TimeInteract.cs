using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeInteract : Interactable
{
    public TimeController timeController;
    public GameObject timeInteractionBox;
    public GameObject timeInteractText;
    public ScreenFade screenFade;

    public AudioClip doorbangSound;
    private AudioSource audioSource;
    public AudioSource backgroundAudio;
    public AudioSource upstairsAudio;
    public float minVolume = 0.1f;
    public float maxVolume = 1.0f;


    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = doorbangSound;
        audioSource.volume = 0.1f;

        if (backgroundAudio != null)
        {
            backgroundAudio.Stop();
        }

        if (upstairsAudio != null)
        {
            upstairsAudio.Stop();
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
        StartUpstairsAudio();
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
            
            InvokeRepeating("RandomiseBackgroundVolume", 2.0f, 2.0f);
        }
    }


    private void RandomiseBackgroundVolume()
    {
        
        float randomVolume = Random.Range(minVolume, maxVolume);

        
        backgroundAudio.volume = randomVolume;
    }

    public void StartUpstairsAudio()
    {
        if (upstairsAudio != null && !upstairsAudio.isPlaying)
        {
            upstairsAudio.Play();

        }
    }

}
