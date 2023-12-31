using System;
using TMPro;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public Light[] lights;
    public float dimmingFactor = 0.01f;
    public int dimStartTime = 20; 
    public int restoreStartTime = 6; 

    private float originalIntensity;
    private bool isDimmed = false;

    private void Start()
    {
      
        originalIntensity = lights[0].intensity;
    }

    private void Update()
    {      
        if (IsNightTime())
        {
            if (!isDimmed)
            {
                DimLights();
                isDimmed = true;
            }
        }
        else
        {
            if (isDimmed)
            {                
                RestoreLights();
                isDimmed = false;
            }
        }
    }

    public bool IsNightTime()
    {
        // parse the time from the text component
        if (DateTime.TryParseExact(timeText.text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime time))
        {
            // check if time is between dimStartTime and restoreStartTime
            return time.Hour >= dimStartTime || time.Hour < restoreStartTime;
        }

        return false;
    }

    public void DimLights()
    {
            foreach (Light light in lights)
            {
                if (light != null)  
                {
                    light.intensity = originalIntensity * dimmingFactor;
                }
            }
        
    }

    public void RestoreLights()
    {        
            foreach (Light light in lights)
            {
                if (light != null)  
                {
                    light.intensity = originalIntensity;
                }
            }        
    }
 
}
