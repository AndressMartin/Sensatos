using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    public void ShowDialogue(Player player)
    {
        if(player.DialogueUI.IsOpen == false)
        {
            UpdateResponseEvents(player, this.dialogueObject);
            player.DialogueUI.UpdateDialogueActivator(this);
            player.DialogueUI.ShowDialogue(dialogueObject);
        }
    }

    public void UpdateResponseEvents(Player player, DialogueObject dialogueObject)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
    }
}
