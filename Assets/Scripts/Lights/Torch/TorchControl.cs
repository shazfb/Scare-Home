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

        if (onoffText == null)
        {
            Debug.LogError("On screen text component not assigned!");
        }
    }

    void OnEnable()
    {
        if (!isTextEnabled)
        {
            StartCoroutine(EnableAndDisableText());
            isTextEnabled = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            torchLight.enabled = !torchLight.enabled;
        }
    }

    public bool IsLightEnabled()
    {
        return torchLight.enabled;
    }
    IEnumerator EnableAndDisableText()
    {
        onoffText.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        onoffText.gameObject.SetActive(false);
    }
}
