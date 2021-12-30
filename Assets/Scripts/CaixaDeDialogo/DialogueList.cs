using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueList : MonoBehaviour
{
    [SerializeField] private List<DialogueObject> dialogueList;

    public DialogueObject GetDialogueObject(string nome)
    {
        foreach(DialogueObject dialogueObject in dialogueList)
        {
            if(dialogueObject.name == nome)
            {
                return dialogueObject;
            }
        }
        Debug.LogWarning("Nao foi possivel achar este dialogo nesta lista. Complicado.");
        return null;
    }
}
