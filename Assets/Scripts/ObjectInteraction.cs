using UnityEngine;

public class ObjectInteraction : Interactable
{
    public GameObject objectToEnable;
    public GameObject objectToDisable;
    public GameObject textObject;
    public GameObject buttontextObject;

    private int interactionCount = 0;



    private void Start()
    {
        textObject.SetActive(false);
        buttontextObject.SetActive(false);
    }

    public override void OnInteract()
    {
        
        ToggleObjects();
    }

    public override void OnFocus()
    {
        if (interactionCount < 1)
        {   
            buttontextObject.SetActive(true); 
        
        }
        
    }

    public override void OnLoseFocus()
    {
        buttontextObject.SetActive(false);
    }

    void ToggleObjects()
    {
        if (objectToEnable != null && objectToDisable != null)
        {
            interactionCount++;

            if (interactionCount > 1)
            {
                if (textObject != null)
                {
                    textObject.SetActive(true);
                    Invoke("DisableTextObject", 2f);
                }
            }
            else
            {
                objectToEnable.SetActive(true);
                objectToDisable.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Make sure all objects are assigned in the inspector.");
        }
    }

    void DisableTextObject()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
    }
}
