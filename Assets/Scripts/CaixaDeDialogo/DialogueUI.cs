using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private GameObject dialogueBox; //Guarda toda a caixa de dialogo
    [SerializeField] private TMP_Text textLabel; //Guarda a caixa de texto
    [SerializeField] private Image portrait; //Guarda o retrato da caixa de dialogo 
    private DialogueActivator dialogueActivator;

    private ResponseEvent dialogueEndEvents;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private DialogueJSONReader dialogueJSONReader;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        dialogueJSONReader = GetComponent<DialogueJSONReader>();

        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        generalManager.PauseManager.SetPermitirInput(false);

        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void CloseDialogueBox()
    {
        generalManager.PauseManager.SetPermitirInput(true);

        IsOpen = false;
        portrait.gameObject.SetActive(false);
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    public void ForcedCloseDialogueBox()
    {
        dialogueEndEvents = null;

        StopAllCoroutines();
        typewriterEffect.Stop();
        responseHandler.Stop();
        CloseDialogueBox();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    public void AddDialogueEndEvents(ResponseEvent responseEvents)
    {
        this.dialogueEndEvents = responseEvents;
    }

    public void UpdateDialogueActivator(DialogueActivator dialogueActivator)
    {
        this.dialogueActivator = dialogueActivator;
    }

    public void CallUpdateResponseEvents(DialogueObject dialogueObject)
    {
        dialogueActivator.UpdateResponseEvents(generalManager, dialogueObject);
    }

    //Atualiza a imagem do retrato e a borda esquerda da caixa de texto
    private void UpdateImage(DialogueObject.DialogueStruct dialogue)
    {
        if (dialogue.portrait != null)
        {
            textLabel.margin = new Vector4(dialogue.portrait.rect.width + 5, 0, 0, 0);
            portrait.gameObject.SetActive(true);
            portrait.sprite = dialogue.portrait;
        }
        else
        {
            textLabel.margin = new Vector4(0, 0, 0, 0);
            portrait.gameObject.SetActive(false);
        }
    }

    //Passa por cada um dos arrays de texto do dialogueObject e os mostra na tela
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        bool achouArquivoDeTexto = dialogueJSONReader.CarregarDialogo(dialogueObject);

        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue;

            UpdateImage(dialogueObject.Dialogue[i]);

            //Confere se um arquivo de texto com o dialogo foi encontrado, e usa o texto dele. Caso nao tenha sido encontrado, usa o texto que esta no DialogueObject
            if(achouArquivoDeTexto == true && dialogueJSONReader.dataDeDialogo.dialogos.Length > i)
            {
                dialogue = dialogueJSONReader.dataDeDialogo.dialogos[i].texto;
            }
            else
            {
                dialogue = dialogueObject.Dialogue[i].text;
            }
            

            yield return RunTypingEffect(dialogue);

            textLabel.maxVisibleCharacters = dialogue.Length;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => InputManager.AvancarDialogo());
        }

        if(dialogueObject.HasResponses)
        {
            dialogueEndEvents = null;
            responseHandler.ShowResponses(dialogueObject.Responses, dialogueJSONReader.dataDeDialogo, achouArquivoDeTexto);
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

            if(InputManager.AvancarDialogo())
            {
                typewriterEffect.Stop();
            }
        }
    }
}
