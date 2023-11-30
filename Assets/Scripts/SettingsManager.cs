using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class SettingsManager : MonoBehaviour
{
    private Volume volume;
    
    //public VolumeProfile volumeProfile;
    //public float minAberrationIntensity = 0.1f;
    //public float maxAberrationIntensity = 1.0f;
    //public float lerpSpeed = 0.5f;

    //private ChromaticAberration chromaticAberration;

    void Start()
    {
        
        volume = GetComponent<Volume>();

       
        if (volume != null)
        {
            
            volume.weight = 1f;
        }
        else
        {
            Debug.LogError("Volume component not found!");
        }
        
    //    // Check if the VolumeProfile is assigned
    //    if (volumeProfile == null)
    //    {
    //        Debug.LogError("Volume Profile not assigned!");
    //        return;
    //    }

    //    // Get the ChromaticAberration effect from the profile
    //    if (volumeProfile.TryGet(out chromaticAberration))
    //    {
    //        // Start lerping the chromatic aberration intensity
    //        StartCoroutine(LerpChromaticAberration());
    //    }
    //    else
    //    {
    //        Debug.LogError("Chromatic Aberration not found in the Volume Profile!");
    //    }
    //}

 

    //IEnumerator LerpChromaticAberration()
    //{
    //    float t = 0f;

    //    while (true)
    //    {
    //        // Lerping the chromatic aberration intensity between min and max
    //        chromaticAberration.intensity.value = Mathf.Lerp(minAberrationIntensity, maxAberrationIntensity, t);

    //        // Increment time for lerping
    //        t += lerpSpeed * Time.deltaTime;

    //        // Wrap around t to keep it in the [0, 1] range
    //        t = Mathf.Repeat(t, 1f);

    //        // Wait for the next frame
    //        yield return null;
    //    }
    }
}
