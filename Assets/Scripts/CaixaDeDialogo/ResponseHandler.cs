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

    public void ShowResponses(Response[] responses)
    {
        float responseBoxHeight = 0;

        for(int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
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

            if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if(selection > 0)
                {
                    selection--;
                    UpdateButtonSelectionEffect(selection);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if(selection < responses.Length - 1)
                {
                    selection++;
                    UpdateButtonSelectionEffect(selection);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
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
                tempResponseButtons[i].GetComponent<TMP_Text>().color = new Color(255, 255, 255, 1);
            }
            else
            {
                tempResponseButtons[i].GetComponent<TMP_Text>().color = new Color(0, 0, 0, 1);
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
