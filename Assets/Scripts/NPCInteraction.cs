using System.Collections;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Dialogue dialogue; // Reference to the Dialogue script
    public Animator npcAnimator; // Reference to the NPC Animator

    void Start()
    {
        dialogue.gameObject.SetActive(false); // Ensure the dialogue box is initially disabled
    }

    void Update()
    {
        

        // Check for player interaction
        if (Input.GetKeyDown(KeyCode.T) && IsPlayerInRange())
        {
            StartDialogue();
        }
        // Check if inDialogue is false in the Dialogue script
        if (!dialogue.IsInDialogue())
        {
            EndDialogue();
            return; // Exit the Update method to avoid unnecessary checks
        }
    }


    private bool IsPlayerInRange()
    {
        // You may need to adjust the collider type or size based on your NPC's setup
        Collider[] colliders = Physics.OverlapSphere(transform.position, 6f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    public void StartDialogue()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.StartDialogue();

        // Check if the NPC has an Animator
        if (npcAnimator != null)
        {
            // Set the inDialogue parameter to true in the Animator
            npcAnimator.SetBool("inDialogue", true);
        }
    }

    // Call this method when the dialogue ends
    public void EndDialogue()
    {
        // Check if the NPC has an Animator
        if (npcAnimator != null)
        {
            // Set the inDialogue parameter to false in the Animator
            npcAnimator.SetBool("inDialogue", false);
            Debug.Log("Set inDialogue parameter to false in Animator");
        }
    }
}
