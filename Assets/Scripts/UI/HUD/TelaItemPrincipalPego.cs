using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TelaItemPrincipalPego : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform tela;

    [SerializeField] private TMP_Text nomeDoItem;
    [SerializeField] private Image imagemDoItem;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        animacao = GetComponent<Animator>();

        tela.gameObject.SetActive(false);
    }

    private void AtualizarInformacoes(ItemDeAssalto item)
    {
        nomeDoItem.text = item.Nome;
        imagemDoItem.sprite = item.Imagem;
    }

    public void IniciarTela(ItemDeAssalto item)
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.TelaItemPrincipalPego);

        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);

        AtualizarInformacoes(item);

        animacao.Play("ItemPego");

        tela.gameObject.SetActive(true);
    }

    public void DesativarTela()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);

        animacao.Play("Vazio");

        tela.gameObject.SetActive(false);
    }

    public void LiberarInput()
    {
        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);
    }

    public void ItemPrincipalColetado()
    {
        generalManager.AssaltoInfo.ItemPrincipalColetado();
    }
}
