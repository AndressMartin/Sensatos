using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    //Componentes
    [SerializeField] private GameObject dialogueBox; //Guarda toda a caixa de dialogo
    [SerializeField] private TMP_Text textLabel; //Guarda a caixa de texto
    private DialogueActivator dialogueActivator;

    private ResponseEvent dialogueEndEvents;
    private Player player;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    void Start()
    {
        //Componentes
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        player = FindObjectOfType<Player>();

        CloseDialogueBox();
    }

    public void UpdateDialogueActivator(DialogueActivator dialogueActivator)
    {
        this.dialogueActivator = dialogueActivator;
    }

    public void CallUpdateResponseEvents(DialogueObject dialogueObject)
    {
        dialogueActivator.UpdateResponseEvents(player, dialogueObject);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    public void AddDialogueEndEvents(ResponseEvent responseEvents)
    {
        this.dialogueEndEvents = responseEvents;
    }

    //Passa por cada um dos arrays de texto do dialogueObject e os mostra na tela
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i].text;

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if(dialogueObject.HasResponses)
        {
            dialogueEndEvents = null;
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            dialogueEndEvents?.OnPickedResponse?.Invoke();
            dialogueEndEvents = null;
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning)
        {
            yield return null;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                typewriterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
