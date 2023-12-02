using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TorchControl : MonoBehaviour
{
    public Light torchLight;
    public GameObject onoffText;
    private bool isTextEnabled = false;

    void Start()
    {
        if (torchLight == null)
        {
            Debug.LogError("Light component not assigned!");
        }

        // Ensure that the onScreenText component is assigned in the Inspector
        if (onoffText == null)
        {
            Debug.LogError("On-screen text component not assigned!");
        }
    }

    void OnEnable()
    {
        // Enable on-screen text for the first time the object is enabled
        if (!isTextEnabled)
        {
            StartCoroutine(EnableAndDisableText());
            isTextEnabled = true;
        }
    }

    void Update()
    {
        // Check for the "F" key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Toggle the light state
            torchLight.enabled = !torchLight.enabled;
        }
    }

    public bool IsLightEnabled()
    {
        return torchLight.enabled;
    }
    IEnumerator EnableAndDisableText()
    {
        // Enable on-screen text
        onoffText.gameObject.SetActive(true);

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Disable on-screen text after 5 seconds
        onoffText.gameObject.SetActive(false);
    }
}
