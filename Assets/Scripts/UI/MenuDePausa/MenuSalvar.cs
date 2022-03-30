using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSalvar : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private SaveSlot[] saveSlots;
    [SerializeField] private PainelDeEscolha opcoesSobrescreverSave;
    [SerializeField] private PainelDeEscolha painelSaveSucesso;

    //Enuns
    public enum Menu { Inicio, ConfirmandoSobrescreverSave, SaveSucesso }

    //Variaveis
    private int selecao;
    private int selecao2;

    private SaveData.SaveFile informacoesSave = new SaveData.SaveFile();

    [SerializeField] private string nomeSlot;
    [SerializeField] private string nomeSlotVazio;

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

        //Variaveis
        selecao = 0;
        selecao2 = 0;

        menuAtual = Menu.Inicio;

        iniciado = true;
    }

    private void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;

        switch(this.menuAtual)
        {
            case Menu.Inicio:
                opcoesSobrescreverSave.gameObject.SetActive(false);
                painelSaveSucesso.gameObject.SetActive(false);
                break;

            case Menu.ConfirmandoSobrescreverSave:
                opcoesSobrescreverSave.gameObject.SetActive(true);
                painelSaveSucesso.gameObject.SetActive(false);
                break;

            case Menu.SaveSucesso:
                opcoesSobrescreverSave.gameObject.SetActive(false);
                painelSaveSucesso.gameObject.SetActive(true);
                break;
        }
    }

    public void IniciarScrool()
    {
        selecao = 0;
        selecao2 = 0;

        for (int i = 0; i < saveSlots.Length; i++)
        {
            if(Save.SaveExiste(i + 1))
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

            case Menu.ConfirmandoSobrescreverSave:
                ConfirmandoSobrescreverSave();
                break;

            case Menu.SaveSucesso:
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
            generalManager.Hud.MenuDePausa.SetMenuAtual(MenuDePausa.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            if(saveSlots[selecao].SaveExiste == true)
            {
                SetMenuAtual(Menu.ConfirmandoSobrescreverSave);

                selecao2 = 0;
                AtualizarPainelDeEscolha(opcoesSobrescreverSave, selecao2);

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
            }
            else
            {
                SalvarJogo(selecao + 1);
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
                    SalvarJogo(selecao + 1);

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
            generalManager.Hud.MenuDePausa.SetMenuAtual(MenuDePausa.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
        }
    }

    private void SalvarJogo(int slot)
    {
        SaveManager.instance.SalvarJogo(slot);

        SetMenuAtual(Menu.SaveSucesso);

        AtualizarPainelDeEscolha(painelSaveSucesso, 0);

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.SalvouComSucesso);
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }
}
