using UnityEngine;
using System;

public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ResponseEvent[] events;

    //Getter
    public DialogueObject DialogueObject => dialogueObject;
    public ResponseEvent[] Events => events;

    //OnValidate roda sempre que esse script e carregado ou um valor dele e mudado no inspetor
    public void OnValidate()
    {
        //Comfere se e necessario atualizar a lista de eventos
        if (dialogueObject == null) return;
        if (dialogueObject.Responses == null) return;
        if (events != null && events.Length == dialogueObject.Responses.Length) return;

        //Cria a lista de eventos caso ela seja nula, e so muda o tamanho caso ela ja exista
        if(events == null)
        {
            events = new ResponseEvent[dialogueObject.Responses.Length];
        }
        else
        {
            Array.Resize(ref events, dialogueObject.Responses.Length);
        }

        for (int i = 0; i < dialogueObject.Responses.Length; i++)
        {
            Response response = dialogueObject.Responses[i];

            //Muda o nome do item na lista caso ele nao seja nulo
            if (events[i] != null)
            {
                events[i].name = response.ResponseText;
                continue; //Ignora as linha de codigo abaixo e pula para a proxima iteracao do for
            }

            //Cria um novo item na lista de eventos
            events[i] = new ResponseEvent() { name = response.ResponseText };
        }
    }
}
