using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;
    public GameObject OpenDoorText;
    public GameObject boxColliderGameObject;

    private void Start()
    {
        anim = GetComponent<Animator>();
        OpenDoorText.SetActive(false);
    }
    public override void OnFocus()
    {
        OpenDoorText.SetActive(true);
    }

    public override void OnInteract()
    {
        if (canBeInteractedWith)
        {
            if (canBeInteractedWith)
                {
                    isOpen = !isOpen;

                    Vector3 doorTransformDirection = transform.TransformDirection(Vector3.forward);
                    Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
                    float dot = Vector3.Dot(doorTransformDirection, playerTransformDirection);

                    anim.SetFloat("dot", dot);
                    anim.SetBool("isOpen", isOpen);

                    if (boxColliderGameObject != null)
                    {
                        BoxCollider boxCollider = boxColliderGameObject.GetComponent<BoxCollider>();
                        if (boxCollider != null)
                        {
                            boxCollider.enabled = !isOpen;
                        }
                        else
                        {
                            Debug.LogError("BoxCollider component not found on the specified GameObject.");
                        }
                    }

                    OpenDoorText.SetActive(false);

                    StartCoroutine(AutoClose());
                            }
        }
    }

    public override void OnLoseFocus()
    {
        OpenDoorText.SetActive(false);
    }

    private IEnumerator AutoClose()
    {
        while (isOpen)
        {
            yield return new WaitForSeconds(3);

            if(Vector3.Distance(transform.position, FirstPersonController.instance.transform.position) > 3)
            {
                isOpen=false;
                anim.SetFloat("dot", 0);
                anim.SetBool("isOpen", isOpen);

            }

            if (boxColliderGameObject != null)
            {
                BoxCollider boxCollider = boxColliderGameObject.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = !isOpen;
                }
                else
                {
                    Debug.LogError("BoxCollider component not found on the specified GameObject.");
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


