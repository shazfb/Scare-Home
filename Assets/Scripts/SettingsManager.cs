using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class SettingsManager : MonoBehaviour
{
    private Volume volume;

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
    }
}
