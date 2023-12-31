using UnityEngine;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{
    private Volume volume;
    
    private void Start()
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
