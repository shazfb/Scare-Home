        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

public class Cupboard : Interactable
    {
        private bool isOpen = false;
        private bool canBeInteractedWith = true;
        private Animator anim;

        public GameObject OpenDoorText;
        public GameObject CloseDoorText;

    public bool IsOpen => isOpen; //allows other scripts to check state of bool 

    private void Start()
        {
            anim = GetComponent<Animator>();
            OpenDoorText.SetActive(false);
            CloseDoorText.SetActive(false);
    }
        public override void OnFocus()
        {
            if (!isOpen)
            {
            OpenDoorText.SetActive(true);
            }
            else if (isOpen)
            {
            CloseDoorText.SetActive(true);
            }
        }

        public override void OnInteract()
        {

            if (canBeInteractedWith)
            {
                isOpen = !isOpen;

                Vector3 cupboardTransformDirection = transform.TransformDirection(Vector3.forward);
                Vector3 playerTransformDirection = FirstPersonController.instance.transform.position - transform.position;
                float dot = Vector3.Dot(cupboardTransformDirection, playerTransformDirection);

                anim.SetFloat("dot", dot);
                anim.SetBool("isOpen", isOpen);


            }
        }

        public override void OnLoseFocus()
        {
            CloseDoorText.SetActive(false);
            OpenDoorText.SetActive(false);
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


