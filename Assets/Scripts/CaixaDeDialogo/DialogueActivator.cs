using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    public void ShowDialogue(GeneralManagerScript generalManager)
    {
        if(generalManager.DialogueUI.IsOpen == false)
        {
            UpdateResponseEvents(generalManager, this.dialogueObject);
            generalManager.DialogueUI.UpdateDialogueActivator(this);
            generalManager.DialogueUI.ShowDialogue(dialogueObject);
        }
    }

    public void UpdateResponseEvents(GeneralManagerScript generalManager, DialogueObject dialogueObject)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                generalManager.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        foreach (DialogueEndEvents dialogueEndEvents in GetComponents<DialogueEndEvents>())
        {
            if (dialogueEndEvents.DialogueObject == dialogueObject)
            {
                generalManager.DialogueUI.AddDialogueEndEvents(dialogueEndEvents.Events);
                break;
            }
        }
    }
}
