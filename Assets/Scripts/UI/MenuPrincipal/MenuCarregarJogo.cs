using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCarregarJogo : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private MenuPrincipal menuPrincipal;

    //Componentes
    [SerializeField] private SaveSlot[] saveSlots;
    [SerializeField] private PainelDeEscolha opcoesCarregarSave;
    [SerializeField] private PainelDeEscolha painelCarregarFalhou;

    //Enuns
    public enum Menu { Inicio, ConfirmandoCarregarSave, CarregarFalhou }

    //Variaveis
    private int selecao;
    private int selecao2;

    private SaveData.SaveFile informacoesSave = new SaveData.SaveFile();

    private string nomeSlot = "Espaço de Salvamento";
    private string nomeSlotVazio = "Espaço de Salvamento Vazio";

    [SerializeField] private string nomeSlotPortugues;
    [SerializeField] private string nomeSlotVazioPortugues;

    [SerializeField] private string nomeSlotIngles;
    [SerializeField] private string nomeSlotVazioIngles;

    private Menu menuAtual;

    private bool iniciado = false;

    //Setters
    public void SetNomeSlot(string novoTexto)
    {
        nomeSlot = novoTexto;
    }

    public void SetNomeSlotVazio(string novoTexto)
    {
        nomeSlotVazio = novoTexto;
    }

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

        menuPrincipal = FindObjectOfType<MenuPrincipal>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        selecao = 0;
        selecao2 = 0;

        menuAtual = Menu.Inicio;

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();

        iniciado = true;
    }

    private void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch (this.menuAtual)
        {
            case Menu.Inicio:
                opcoesCarregarSave.gameObject.SetActive(false);
                painelCarregarFalhou.gameObject.SetActive(false);
                break;

            case Menu.ConfirmandoCarregarSave:
                opcoesCarregarSave.gameObject.SetActive(true);
                painelCarregarFalhou.gameObject.SetActive(false);
                break;

            case Menu.CarregarFalhou:
                opcoesCarregarSave.gameObject.SetActive(false);
                painelCarregarFalhou.gameObject.SetActive(true);
                break;
        }
    }

    public void IniciarScrool()
    {
        selecao = 0;
        selecao2 = 0;

        for (int i = 0; i < saveSlots.Length; i++)
        {
            if (Save.SaveExiste(i + 1))
            {
                informacoesSave = Save.CarregarInformacoes(i + 1);
                saveSlots[i].AtualizarInformacoes(informacoesSave, nomeSlot + " " + (i + 1).ToString());
            }
            else
            {
                saveSlots[i].ZerarInformacoes(nomeSlotVazio);
            }
        }

        SetMenuAtual(Menu.Inicio);

        AtualizarScrool();
    }

    private void AtualizarScrool()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.Selecionado(false);
        }

        saveSlots[selecao].Selecionado(true);
    }

    public void EscolhendoSave()
    {
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.ConfirmandoCarregarSave:
                ConfirmandoCarregarSave();
                break;

            case Menu.CarregarFalhou:
                ConfirmarParaVoltar();
                break;
        }
    }

    private void MenuInicial()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao > 0)
            {
                selecao--;

                AtualizarScrool();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < saveSlots.Length - 1)
            {
                selecao++;

                AtualizarScrool();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            menuPrincipal.SetMenuAtual(MenuPrincipal.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            if (saveSlots[selecao].SaveExiste == true)
            {
                SetMenuAtual(Menu.ConfirmandoCarregarSave);

                selecao2 = 0;
                AtualizarPainelDeEscolha(opcoesCarregarSave, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
            }
            else
            {
                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
            }
        }
    }

    private void ConfirmandoCarregarSave()
    {
        //Mover para cima
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(opcoesCarregarSave, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Direita())
        {
            if (selecao2 < opcoesCarregarSave.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(opcoesCarregarSave, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            SetMenuAtual(Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            switch (selecao2)
            {
                case 0:
                    SetMenuAtual(Menu.Inicio);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
                    break;

                case 1:
                    CarregarJogo(selecao + 1);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void ConfirmarParaVoltar()
    {
        //Continuar
        if (InputManager.Voltar() || InputManager.Confirmar())
        {
            menuPrincipal.SetMenuAtual(MenuPrincipal.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    private void CarregarJogo(int slot)
    {
        bool conseguiuCarregarJogo;

        conseguiuCarregarJogo = SaveManager.instance.CarregarJogo(slot);

        if(conseguiuCarregarJogo == true)
        {
            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.SalvouComSucesso);

            IniciarJogo();
        }
        else
        {
            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);

            SetMenuAtual(Menu.CarregarFalhou);

            AtualizarPainelDeEscolha(painelCarregarFalhou, 0);
        }

    }

    private void IniciarJogo()
    {
        menuPrincipal.IniciarJogo();
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    private void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                nomeSlot = nomeSlotPortugues;
                nomeSlotVazio = nomeSlotVazioPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                nomeSlot = nomeSlotIngles;
                nomeSlotVazio = nomeSlotVazioIngles;
                break;
        }
    }
}
