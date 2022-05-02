using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LojaDeArmas : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private ListaSlot[] armas;

    [SerializeField] private TMP_Text nomeDaArma;
    [SerializeField] private TMP_Text descricaoDaArma;

    [SerializeField] private RectTransform barraDeRolagem;

    //Variaveis
    private int selecao;
    private int scrool;

    private float barraDeRolagemAlturaInicial;

    private bool iniciado = false;

    void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        selecao = 0;
        scrool = 0;

        barraDeRolagemAlturaInicial = barraDeRolagem.sizeDelta.y;

        foreach (ListaSlot listaSlot in armas)
        {
            listaSlot.Iniciar();
            listaSlot.ZerarInformacoes();
        }

        iniciado = true;
    }

    private void AtualizarInformacoesDaArma()
    {
        nomeDaArma.text = generalManager.Player.InventarioMissao.Itens[selecao].Nome;
        descricaoDaArma.text = generalManager.Player.InventarioMissao.Itens[selecao].Descricao;
    }

    private void AtualizarScroolDosItens()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            if (scrool + i >= generalManager.Player.InventarioMissao.Itens.Count || scrool + i < 0)
            {
                armas[i].gameObject.SetActive(false);
            }
            else
            {
                armas[i].AtualizarNome(generalManager.Player.InventarioMissao.Itens[scrool + i].Nome);
                armas[i].AtualizarNumero(generalManager.Player.InventarioMissao.Itens[scrool + i].Quantidade);
                armas[i].gameObject.SetActive(true);
            }
        }

        foreach (ListaSlot listaSlot in armas)
        {
            listaSlot.Selecionado(false);
        }

        armas[selecao - scrool].Selecionado(true);

        //Posicao da Barra de Rolagem
        if (armas.Length < generalManager.Player.InventarioMissao.Itens.Count)
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / generalManager.Player.InventarioMissao.Itens.Count) * scrool * -1);
        }
        else
        {
            barraDeRolagem.anchoredPosition = Vector2.zero;
        }
    }

    public void IniciarScrool()
    {
        selecao = 0;
        scrool = 0;

        //Tamanho da Barra de Rolagem
        if (armas.Length < generalManager.Player.InventarioMissao.Itens.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * ((float)armas.Length / generalManager.Player.InventarioMissao.Itens.Count));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }

        AtualizarScroolDosItens();

        AtualizarInformacoesDaArma();

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuItensChave()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao > 0)
            {
                selecao--;

                if (selecao < scrool)
                {
                    scrool = selecao;
                }

                AtualizarScroolDosItens();
                AtualizarInformacoesDaArma();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < generalManager.Player.InventarioMissao.Itens.Count - 1)
            {
                selecao++;

                if (selecao - scrool > armas.Length - 1)
                {
                    scrool = selecao - (armas.Length - 1);
                }

                AtualizarScroolDosItens();
                AtualizarInformacoesDaArma();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }
    }
}
