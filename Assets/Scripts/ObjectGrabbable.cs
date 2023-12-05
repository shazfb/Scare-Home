using System.Collections;
using UnityEngine;

public class ObjectGrabbable : Interactable
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    private Transform objectGrabPointTransform2;
    private FirstPersonController firstPersonController;

    public GameObject interactText;

    public string snapToTag = "SnapObject";

    public override void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        interactText.SetActive(false);
        firstPersonController = FirstPersonController.instance; 
    }

    
    public void Grab(Transform objectGrabPointTransform, Transform objectGrabPointTransform2)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        this.objectGrabPointTransform2 = objectGrabPointTransform2;
        objectRigidBody.useGravity = false;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Transform targetGrabPoint = firstPersonController.isIdle ? objectGrabPointTransform : objectGrabPointTransform2;
            float interpolationFactor = lerpSpeed * Time.fixedDeltaTime;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetGrabPoint.position, interpolationFactor);
            objectRigidBody.MovePosition(newPosition);
        }
    }

public override void OnFocus()
    {
        if (objectGrabPointTransform == null)
        {
            interactText.SetActive(true);
        }
    }

    public override void OnLoseFocus()
    {
       
        interactText.SetActive(false);
    }

    public override void OnInteract()
    {
        // check if the object is currently being grabbed or not
        if (objectGrabPointTransform == null)
        {
            // if not, grab the object
            if (Input.GetKeyDown(KeyCode.E))
            {
                Grab(FirstPersonController.instance.transform, FirstPersonController.instance.transform);
            }
        }
        else
        {
            // if already grabbed, drop the object
            if (Input.GetKeyDown(KeyCode.E))
            {
                Drop();
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        // check if the object is dropped and has a collider underneath
        if (objectGrabPointTransform == null && collision.contacts.Length > 0)
        {
            // check if the collided object has the specified tag
            if (collision.collider.CompareTag(snapToTag))
            {
                // snap the object to the center of the collided object
                SnapToCenter(collision.collider);
            }
        }
    }

    private void SnapToCenter(Collider targetCollider)
    {
        // set the objects position to the center of the target object
        objectRigidBody.MovePosition(targetCollider.bounds.center);
    }
}
