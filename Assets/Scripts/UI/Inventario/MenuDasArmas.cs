using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuDasArmas : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private RectTransform scroolDasArmas;
    [SerializeField] private SelecaoArma[] armas;

    [SerializeField] private TMP_Text nomeDaArma;
    [SerializeField] private TMP_Text descricaoDaArma;
    [SerializeField] private InformacoesDaMelhoria[] melhorias;

    [SerializeField] private SetaDeScrool[] setasDeScrool;

    //Variaveis
    private int indiceArmaAtual;
    private int selecao;
    private int scrool;

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
        indiceArmaAtual = 0;
        selecao = 0;
        scrool = 0;

        foreach(SelecaoArma selecaoArma in armas)
        {
            selecaoArma.Iniciar();
        }

        foreach(SetaDeScrool setaDeScrool in setasDeScrool)
        {
            setaDeScrool.Ativa(false);
        }

        iniciado = true;
    }

    private void AtualizarInformacoesDaArma()
    {
        nomeDaArma.text = generalManager.Player.Inventario.Armas[selecao].Nome;
        descricaoDaArma.text = generalManager.Player.Inventario.Armas[selecao].Descricao;

        foreach(InformacoesDaMelhoria melhoria in melhorias)
        {
            melhoria.gameObject.SetActive(false);
        }

        for (int i = 0; i < generalManager.Player.Inventario.Armas[selecao].Melhorias.Count; i++)
        {
            melhorias[i].gameObject.SetActive(true);
            melhorias[i].AtualizarInformacoes(generalManager.Player.Inventario.Armas[selecao].Melhorias[i]);
        }
    }

    public void AtualizarPosicaoDoScroolDasArmas(float posY)
    {
        scroolDasArmas.transform.position = new Vector2(scroolDasArmas.transform.position.x, posY);
    }

    private void AtualizarScroolDasArmas()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            if(scrool + i >= generalManager.Player.Inventario.Armas.Count || scrool + i < 0)
            {
                armas[i].ZerarInformacoes();
            }
            else
            {
                armas[i].AtualizarInformacoes(generalManager.Player.Inventario.Armas[scrool + i]);
            }
        }

        foreach(SelecaoArma arma in armas)
        {
            arma.Selecionado(false);
        }

        armas[selecao - scrool].Selecionado(true);


        //Setas de Scrool
        if(scrool > 0)
        {
            setasDeScrool[0].Ativa(true);
        }
        else
        {
            setasDeScrool[0].Ativa(false);
        }

        if (scrool + armas.Length < generalManager.Player.Inventario.Armas.Count)
        {
            setasDeScrool[1].Ativa(true);
        }
        else
        {
            setasDeScrool[1].Ativa(false);
        }
    }

    public void IniciarScrool(int novoIndiceArmaAtual)
    {
        indiceArmaAtual = novoIndiceArmaAtual;

        for(int i = 0; i < generalManager.Player.Inventario.Armas.Count; i++)
        {
            if(generalManager.Player.Inventario.Armas[i] == generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual])
            {
                selecao = i;
                scrool = i;
                break;
            }
        }

        AtualizarScroolDasArmas();
        AtualizarInformacoesDaArma();

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuArma()
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

                AtualizarScroolDasArmas();
                AtualizarInformacoesDaArma();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < generalManager.Player.Inventario.Armas.Count - 1)
            {
                selecao++;

                if (selecao - scrool > armas.Length - 1)
                {
                    scrool = selecao - (armas.Length - 1);
                }

                AtualizarScroolDasArmas();
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

        //Confirmar
        if (InputManager.Confirmar())
        {
            ConfirmarArma();

            generalManager.Hud.MenuDoInventario.SetMenuAtual(MenuDoInventario.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.EquiparArma);
        }
    }

    public void ConfirmarArma()
    {
        int indiceComparacao;
        ArmaDeFogo armaTemp;

        if(indiceArmaAtual == 0)
        {
            indiceComparacao = 1;
        }
        else
        {
            indiceComparacao = 0;
        }

        //Confere se a arma que o jogador vai equipar e igual a outra arma que o jogador tem equipada, se for verdade, inverte as armas equipadas
        if(generalManager.Player.Inventario.Armas[selecao] == generalManager.Player.Inventario.ArmaSlot[indiceComparacao])
        {
            armaTemp = generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual];

            generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual] = generalManager.Player.Inventario.Armas[selecao];
            generalManager.Player.Inventario.ArmaSlot[indiceComparacao] = armaTemp;
        }
        else
        {
            generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual] = generalManager.Player.Inventario.Armas[selecao];
        }
    }
}
