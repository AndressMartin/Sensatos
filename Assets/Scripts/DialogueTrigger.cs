using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   
    public Dialogue dialogue;
    public bool canDialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void Update()
    {
        canDialogue = GetComponentInChildren<DialogueFeed>().canDialogue;
        if (Input.GetKeyDown(KeyCode.E) && canDialogue)
        {
            TriggerDialogue();
        }
     
    }
}
