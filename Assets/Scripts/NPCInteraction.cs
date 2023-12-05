using System.Collections;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Dialogue dialogue; 
    public Animator npcAnimator; 

    void Start()
    {
        dialogue.gameObject.SetActive(false); 
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T) && IsPlayerInRange())
        {
            StartDialogue();
        }
        
        if (!dialogue.IsInDialogue())
        {
            EndDialogue();
            return; 
        }
    }


    private bool IsPlayerInRange()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);

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

        
        if (npcAnimator != null)
        {
            
            npcAnimator.SetBool("inDialogue", true);
        }
    }

    
    public void EndDialogue()
    {
        
        if (npcAnimator != null)
        {
            
            npcAnimator.SetBool("inDialogue", false);
            Debug.Log("Set inDialogue parameter to false in Animator");
        }
    }
}
