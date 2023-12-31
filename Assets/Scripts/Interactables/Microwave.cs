using System.Collections;
using UnityEngine;

public class Microwave : Interactable
{
    private bool isOn = false;
    private bool canBeInteractedWith = true;
    private Light interactableLight;
    private AudioSource interactableAudio;

    public GameObject animatorGameObject;

    private Animator otherAnimator;

    public GameObject lightGameObject;
    public GameObject boxColliderGameObject;
    public GameObject boxColliderGameObject2;

    public GameObject cupboarddoorGameObject;

    private Cupboard cupboarddoorScript;

    public GameObject InteractText;
    public GameObject InteracterrorText;

    private void Start()
    {
        interactableAudio = GetComponent<AudioSource>();
        interactableAudio.enabled = isOn;

        if (lightGameObject != null)
        {
            interactableLight = lightGameObject.GetComponent<Light>();
            interactableLight.enabled = isOn;
        }
        else
        {
            Debug.LogError("lightGameObject reference not set. Make sure to assign the GameObject with the Light component in the Inspector.");
        }

        InteractText.SetActive(false);
        InteracterrorText.SetActive(false);

        
        if (cupboarddoorGameObject != null)
        {
            cupboarddoorScript = cupboarddoorGameObject.GetComponent<Cupboard>();
            if (cupboarddoorScript == null)
            {
                Debug.LogError("Cupboard Door script not found on the specified GameObject.");
            }
        }
        else
        {
            Debug.LogError("doorGameObject reference not set. Assign the GameObject with the Door script in the Inspector.");
        }

        if (animatorGameObject != null)
        {
            otherAnimator = animatorGameObject.GetComponent<Animator>();
            if (otherAnimator == null)
            {
                Debug.LogError("Animator component not found on the specified GameObject.");
            }
        }
        else
        {
            Debug.LogError("animatorGameObject reference not set. Assign the GameObject with the Animator component in the Inspector.");
        }
    }

    public override void OnFocus()
    {
        if (cupboarddoorScript.IsOpen)
        {
            InteracterrorText.SetActive(true);
        }
        else
        {
            InteractText.SetActive(true);
        }
    }

    public override void OnInteract()
    {
        if (canBeInteractedWith && cupboarddoorScript != null && !cupboarddoorScript.IsOpen)
        {
            isOn = !isOn;

            if (interactableLight != null)
            {
                interactableLight.enabled = isOn;
            }

            interactableAudio.enabled = isOn;

            if (boxColliderGameObject != null)
            {
                BoxCollider boxCollider = boxColliderGameObject.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = !isOn;
                }
                else
                {
                    Debug.LogError("BoxCollider component not found on the specified GameObject.");
                }
            }

            if (boxColliderGameObject2 != null)
            {
                BoxCollider boxCollider2 = boxColliderGameObject2.GetComponent<BoxCollider>();
                if (boxCollider2 != null)
                {
                    boxCollider2.enabled = !isOn;
                }
                else
                {
                    Debug.LogError("BoxCollider component not found on the specified GameObject.");
                }
            }


            Vector3 objectTransformDirection = transform.TransformDirection(Vector3.forward);
            Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
            InteractText.SetActive(false);

            if (otherAnimator != null)
            {
                otherAnimator.SetBool("isOn", isOn);
            }

            StartCoroutine(AutoOff());
        }
    }

    public override void OnLoseFocus()
    {
        InteractText.SetActive(false);
        InteracterrorText.SetActive(false);
    }

    private IEnumerator AutoOff()
    {
        while (isOn)
        {
            yield return new WaitForSeconds(11);

            if (Vector3.Distance(transform.position, FirstPersonController.instance.transform.position) > 0.1)
            {
                isOn = false;
                otherAnimator.SetBool("isOn", isOn); 

                if (interactableLight != null)
                {
                    interactableLight.enabled = isOn;
                }

                interactableAudio.enabled = isOn;

                if (boxColliderGameObject != null)
                {
                    BoxCollider boxCollider = boxColliderGameObject.GetComponent<BoxCollider>();
                    if (boxCollider != null)
                    {
                        boxCollider.enabled = !isOn;
                    }
                    else
                    {
                        Debug.LogError("BoxCollider component not found on the specified GameObject.");
                    }
                }

                if (boxColliderGameObject2 != null)
                {
                    BoxCollider boxCollider2 = boxColliderGameObject2.GetComponent<BoxCollider>();
                    if (boxCollider2 != null)
                    {
                        boxCollider2.enabled = !isOn;
                    }
                    else
                    {
                        Debug.LogError("BoxCollider component not found on the specified GameObject.");
                    }
                }

                
               
            }
        }
    }

    private void Animator_LockInteraction()
    {
        canBeInteractedWith = false;
    }

    private void Animator_UnlockInteraction()
    {
        canBeInteractedWith = true;

    }
}
