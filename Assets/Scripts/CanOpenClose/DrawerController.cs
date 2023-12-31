using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : Interactable
{
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;
    public GameObject torchObject; 
    public GameObject playerTorch;
    public GameObject interactText;
   

    public bool IsOpen => isOpen; //allows other scripts to check state of bool 

    private void Start()
    {
        anim = GetComponent<Animator>();
        interactText.SetActive(false);
        torchObject.SetActive(false);
    }
    public override void OnFocus()
    {

        interactText.SetActive(true);
    }
       

    public override void OnInteract()
    {

        if (canBeInteractedWith)
        {
            isOpen = !isOpen;

            Vector3 drawerTransformDirection = transform.TransformDirection(Vector3.forward);
            Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
            float dot = Vector3.Dot(drawerTransformDirection, playerTransformDirection);

            anim.SetFloat("dot", dot);
            anim.SetBool("isOpen", isOpen);


        }

        if (isOpen && !playerTorch.activeSelf)
        {
            torchObject.SetActive(true);
        }
        else
        {
            torchObject.SetActive(false);
        }
    }

    public override void OnLoseFocus()
    {
        interactText.SetActive(false);
      
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





