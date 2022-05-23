using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    //Componentes
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI dialogueUI;
    private ResponseEvent[] responseEvents;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private Coroutine waitingForResponse;

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void Stop()
    {
        responseBox.gameObject.SetActive(false);
        ClearResponseButtons();
        responseEvents = null;
        if(waitingForResponse != null)
        {
            StopCoroutine(waitingForResponse);
        }
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    public void ShowResponses(Response[] responses, DialogueJSONReader.DataDeDialogo dataDeDialogo, bool achouArquivoDeTexto)
    {
        float responseBoxHeight = 0;

        for(int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);

            //Confere se um arquivo de texto com o texto da resposta foi encontrado, e usa o texto dele. Caso nao tenha sido encontrado, usa o texto que esta no objeto da resposta
            if (achouArquivoDeTexto == true && dataDeDialogo.respostas.Length > i)
            {
                responseButton.GetComponentInChildren<TMP_Text>().text = dataDeDialogo.respostas[i].texto;
            }
            else
            {
                if (achouArquivoDeTexto == true)
                {
                    Debug.LogWarning("Ha menos respostas no arquivo do que no objeto de respostas!");
                }

                responseButton.GetComponentInChildren<TMP_Text>().text = response.ResponseText;
            }

            //responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickResponse(response, responseIndex)); //Adiciona um metodo ao botao, a mesma coisa que se faz atraves do editor do Unity, mas por codigo

            tempResponseButtons.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight); //Seta o tamanho do objeto
        responseBox.gameObject.SetActive(true);

        waitingForResponse = StartCoroutine(ChooseResponse(responses));
    }

    private IEnumerator ChooseResponse(Response[] responses)
    {
        bool responsePicked = false;
        int selection = 0;
        UpdateButtonSelectionEffect(selection);
        while (responsePicked == false)
        {
            yield return null;

            if(InputManager.Cima()) //Mover para cima
            {
                if(selection > 0)
                {
                    selection--;
                    UpdateButtonSelectionEffect(selection);
                }
            }
            else if (InputManager.Baixo()) //Mover para baixo
            {
                if(selection < responses.Length - 1)
                {
                    selection++;
                    UpdateButtonSelectionEffect(selection);
                }
            }
            else if (InputManager.AvancarDialogo()) //Confirmar
            {
                responsePicked = true;
            }
        }

        OnPickResponse(responses[selection], selection);
    }

    private void OnPickResponse(Response response, int responseIndex)
    {
        responseBox.gameObject.SetActive(false);

        ClearResponseButtons();

        if(responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;

        if(response.DialogueObject)
        {
            dialogueUI.CallUpdateResponseEvents(response.DialogueObject);
            dialogueUI.ShowDialogue(response.DialogueObject);
        }
        else
        {
            dialogueUI.CloseDialogueBox();
        }
    }

    private void UpdateButtonSelectionEffect(int selection)
    {
        for (int i = 0; i < tempResponseButtons.Count; i++)
        {
            if(i == selection)
            {
                tempResponseButtons[i].GetComponent<Animator>().SetBool("Selecionado", true);
            }
            else
            {
                tempResponseButtons[i].GetComponent<Animator>().SetBool("Selecionado", false);
            }
        }
    }

    private void ClearResponseButtons()
    {
        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();
    }
}
