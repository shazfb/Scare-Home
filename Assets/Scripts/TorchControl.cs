using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchControl : MonoBehaviour
{
    public Light myLight;

    void Start()
    {
        // Ensure that the light component is assigned in the Inspector
        if (myLight == null)
        {
            Debug.LogError("Light component not assigned!");
        }
    }

    void Update()
    {
        // Check for the "F" key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle the light state
            myLight.enabled = !myLight.enabled;
        }
    }
}
