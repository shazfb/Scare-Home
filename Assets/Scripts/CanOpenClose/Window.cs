using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Interactable
{
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;
    public GameObject WindowInteractText;

    private void Start()
    {
        anim = GetComponent<Animator>();
        WindowInteractText.SetActive(false);
    }
    public override void OnFocus()
    {
        WindowInteractText.SetActive(true);
    }

    public override void OnInteract()
    {

        if (canBeInteractedWith)
        {
            isOpen = !isOpen;

            Vector3 windowTransformDirection = transform.TransformDirection(Vector3.forward);
            Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
            float dot = Vector3.Dot(windowTransformDirection, playerTransformDirection);

            anim.SetFloat("dot", dot);
            anim.SetBool("isOpen", isOpen);

            
        }
    }

    public override void OnLoseFocus()
    {
        WindowInteractText.SetActive(false);
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


