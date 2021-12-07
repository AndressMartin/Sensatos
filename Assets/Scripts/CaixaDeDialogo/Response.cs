using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;

    //Getters
    public string ResponseText => responseText;
    public DialogueObject DialogueObject => dialogueObject;
}
