using UnityEngine;
using System;

public class DialogueEndEvents : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent events;

    //Getter
    public DialogueObject DialogueObject => dialogueObject;
    public ResponseEvent Events => events;
}