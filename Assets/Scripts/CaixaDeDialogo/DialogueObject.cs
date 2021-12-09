using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField] private DialogueStruct[] dialogue;
    [SerializeField] private Response[] responses;

    //Getters
    public DialogueStruct[] Dialogue => dialogue;
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;

    [System.Serializable]
    public struct DialogueStruct
    {
        [SerializeField] public Sprite portrait;
        [SerializeField] [TextArea] public string text;
    }
}
