using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNovoJogo : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    private MenuPrincipal menuPrincipal;

    //Componentes
    [SerializeField] private SaveSlot[] saveSlots;
    [SerializeField] private PainelDeEscolha continuarSemSalvar;

    [SerializeField] private PainelDeEscolha opcoesSobrescreverSave;
    [SerializeField] private PainelDeEscolha opcoesContinuarSemSalvar;

    //Enuns
    public enum Menu { Inicio, ConfirmandoSobrescreverSave, ConfirmandoContinuarSemSalvar}

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
                opcoesSobrescreverSave.gameObject.SetActive(false);
                opcoesContinuarSemSalvar.gameObject.SetActive(false);
                break;

            case Menu.ConfirmandoSobrescreverSave:
                opcoesSobrescreverSave.gameObject.SetActive(true);
                opcoesContinuarSemSalvar.gameObject.SetActive(false);
                break;

            case Menu.ConfirmandoContinuarSemSalvar:
                opcoesSobrescreverSave.gameObject.SetActive(false);
                opcoesContinuarSemSalvar.gameObject.SetActive(true);
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

        continuarSemSalvar.SemSelecao();

        if(selecao < saveSlots.Length)
        {
            saveSlots[selecao].Selecionado(true);
        }
        else
        {
            AtualizarPainelDeEscolha(continuarSemSalvar, 0);
        }
    }

    public void EscolhendoSave()
    {
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;

            case Menu.ConfirmandoSobrescreverSave:
                ConfirmandoSobrescreverSave();
                break;

            case Menu.ConfirmandoContinuarSemSalvar:
                ConfirmandoContinuarSemSalvar();
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
            //Caso a selecao esteja igual ao tamanho da lista, significa que ela esta na opcao de continuar sem salvar
            if (selecao < saveSlots.Length)
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
            if(selecao < saveSlots.Length)
            {
                if (saveSlots[selecao].SaveExiste == true)
                {
                    SetMenuAtual(Menu.ConfirmandoSobrescreverSave);

                    selecao2 = 0;
                    AtualizarPainelDeEscolha(opcoesSobrescreverSave, selecao2);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                }
                else
                {
                    EscolherSlotDeSave(selecao + 1);
                }
            }
            else
            {
                SetMenuAtual(Menu.ConfirmandoContinuarSemSalvar);

                selecao2 = 0;
                AtualizarPainelDeEscolha(opcoesContinuarSemSalvar, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
            }
        }
    }

    private void ConfirmandoSobrescreverSave()
    {
        //Mover para cima
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(opcoesSobrescreverSave, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Direita())
        {
            if (selecao2 < opcoesSobrescreverSave.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(opcoesSobrescreverSave, selecao2);

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
                    EscolherSlotDeSave(selecao + 1);

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void ConfirmandoContinuarSemSalvar()
    {
        //Mover para cima
        if (InputManager.Esquerda())
        {
            if (selecao2 > 0)
            {
                selecao2--;

                AtualizarPainelDeEscolha(opcoesContinuarSemSalvar, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Direita())
        {
            if (selecao2 < opcoesContinuarSemSalvar.Opcoes.Length - 1)
            {
                selecao2++;

                AtualizarPainelDeEscolha(opcoesContinuarSemSalvar, selecao2);

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
                    IniciarSemSalvar();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                    break;
            }
        }
    }

    private void EscolherSlotDeSave(int slot)
    {
        Save.SetSaveAtual(slot);

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.SalvouComSucesso);

        IniciarNovoJogo();
    }

    private void IniciarSemSalvar()
    {
        Save.SetSaveAtual(0);

        IniciarNovoJogo();
    }

    private void IniciarNovoJogo()
    {
        menuPrincipal.IniciarNovoJogo();
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
