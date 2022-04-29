using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuEscolherAssalto : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private Animator animacao;

    [SerializeField] private RectTransform menu;
    [SerializeField] private SetaDeScrool[] setasDeScrool;
    [SerializeField] private PainelDeEscolha botaoSelecionarAssalto;
    [SerializeField] private TMP_Text botaoSelecionarAssaltoTexto;

    [SerializeField] private TMP_Text nomeDoAssalto;
    [SerializeField] private Image imagemDoAssalto;

    [SerializeField] private ListaSlot[] missoes;
    [SerializeField] private Sprite imagemMissaoConcluida;
    [SerializeField] private Sprite imagemMissaoNaoConcluida;

    private string textoSelecionarAssalto;
    private string textoIniciarAssalto;
    private string textoMissoes;

    [SerializeField] private string textoSelecionarAssaltoPortugues;
    [SerializeField] private string textoIniciarAssaltoPortugues;
    [SerializeField] [TextArea(3, 6)] private string textoMissoesPortugues;

    [SerializeField] private string textoSelecionarAssaltoIngles;
    [SerializeField] private string textoIniciarAssaltoIngles;
    [SerializeField] [TextArea(3, 6)] private string textoMissoesIngles;

    //Enums
    public enum Menu { Inicio }

    //Variaveis
    private bool ativo;
    private Menu menuAtual;

    private int selecao;

    //Setters
    public void AtivarMenu()
    {
        ativo = true;

        generalManager.PauseManager.Pausar(true);
        generalManager.PauseManager.SetPermitirInput(false);
    }

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Componentes
        animacao = GetComponent<Animator>();

        //Variaveis
        ativo = false;
        selecao = 0;

        foreach (ListaSlot missao in missoes)
        {
            missao.Iniciar();
            missao.ZerarInformacoes();
        }

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();

        menu.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (ativo == false)
        {
            return;
        }

        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;
        }
    }

    private void IniciarSelecao()
    {
        selecao = 0;

        for (int i = 0; (i < generalManager.AssaltoManager.Assaltos.Count) && (i < GameManager.instance.AssaltosLiberados); i++)
        {
            if (generalManager.AssaltoManager.Assaltos[i] == generalManager.AssaltoManager.GetAssaltoAtual)
            {
                selecao = i;
                break;
            }
        }

        AtualizarTela();
    }

    private void AtualizarTela()
    {
        //Setas de Scrool
        if (selecao > 0)
        {
            setasDeScrool[0].Ativa(true);
        }
        else
        {
            setasDeScrool[0].Ativa(false);
        }

        if ((selecao < generalManager.AssaltoManager.Assaltos.Count - 1) && (selecao < GameManager.instance.AssaltosLiberados - 1))
        {
            setasDeScrool[1].Ativa(true);
        }
        else
        {
            setasDeScrool[1].Ativa(false);
        }

        //Informacoes do Assalto
        nomeDoAssalto.text = generalManager.AssaltoManager.Assaltos[selecao].Nome;
        imagemDoAssalto.sprite = generalManager.AssaltoManager.Assaltos[selecao].Imagem;

        //Missoes do Assalto
        AtualizarScroolDasMissoes();

        //Botao de Iniciar o Assalto
        if (generalManager.AssaltoManager.GetAssaltoAtual == generalManager.AssaltoManager.Assaltos[selecao])
        {
            if(generalManager.AssaltoManager.Assaltos[selecao].MissoesPrincipaisConcluidas() == true)
            {
                botaoSelecionarAssaltoTexto.text = textoIniciarAssalto;

                AtualizarPainelDeEscolha(botaoSelecionarAssalto, 0);
            }
            else
            {
                botaoSelecionarAssaltoTexto.text = textoMissoes;

                botaoSelecionarAssalto.SemSelecao();
            }
        }
        else
        {
            botaoSelecionarAssaltoTexto.text = textoSelecionarAssalto;

            AtualizarPainelDeEscolha(botaoSelecionarAssalto, 0);
        }
    }

    private void AtualizarScroolDasMissoes()
    {
        for (int i = 0; i < missoes.Length; i++)
        {
            if (i < generalManager.AssaltoManager.Assaltos[selecao].GetMissoesPrincipais.Count)
            {
                missoes[i].AtualizarNome(generalManager.AssaltoManager.Assaltos[selecao].GetMissoesPrincipais[i].Nome);

                if(generalManager.AssaltoManager.Assaltos[selecao].GetMissoesPrincipais[i].GetEstado == Missoes.Estado.Concluida)
                {
                    missoes[i].AtualizarImagem(imagemMissaoConcluida);
                }
                else
                {
                    missoes[i].AtualizarImagem(imagemMissaoNaoConcluida);
                }

                missoes[i].gameObject.SetActive(true);
            }
            else
            {
                missoes[i].gameObject.SetActive(false);
            }
        }

        foreach (ListaSlot missao in missoes)
        {
            missao.Selecionado(false);
        }
    }

    private void MenuInicial()
    {
        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao > 0)
            {
                selecao--;

                AtualizarTela();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if ((selecao < generalManager.AssaltoManager.Assaltos.Count - 1) && (selecao < GameManager.instance.AssaltosLiberados - 1))
            {
                selecao++;

                AtualizarTela();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            DesativarMenu();

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            if (generalManager.AssaltoManager.GetAssaltoAtual == generalManager.AssaltoManager.Assaltos[selecao])
            {
                if (generalManager.AssaltoManager.Assaltos[selecao].MissoesPrincipaisConcluidas() == true)
                {
                    IniciarComecarAssalto();

                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
                }
                else
                {
                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
                }
            }
            else
            {
                generalManager.AssaltoManager.SetarAssalto(generalManager.AssaltoManager.Assaltos[selecao]);

                AtualizarTela();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
            }
        }
    }

    private void AtualizarPainelDeEscolha(PainelDeEscolha painelDeEscolha, int selecao)
    {
        painelDeEscolha.Selecionar(selecao);
    }

    public void IniciarMenu()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.EscolherAssalto);
        generalManager.PauseManager.SetPermitirInput(false);

        botaoSelecionarAssalto.SemSelecao();

        IniciarSelecao();

        AtivarMenu();

        menu.gameObject.SetActive(true);
    }

    public void DesativarMenu()
    {
        generalManager.Hud.SetMenuAberto(HUDScript.Menu.Nenhum);

        generalManager.PauseManager.Pausar(false);
        generalManager.PauseManager.SetPermitirInput(true);

        ativo = false;
        animacao.Play("Vazio");
        menu.gameObject.SetActive(false);
    }

    private void IniciarComecarAssalto()
    {
        ativo = false;

        animacao.Play("ComecarAssalto");
    }

    public void ComecarAssalto()
    {
        generalManager.Player.SetInventarioAntesDoAssalto();
        LevelLoaderScript.Instance.CarregarNivel(generalManager.AssaltoManager.GetAssaltoAtual.NomeDaCena);
    }

    public void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                textoSelecionarAssalto = textoSelecionarAssaltoPortugues;
                textoIniciarAssalto = textoIniciarAssaltoPortugues;
                textoMissoes = textoMissoesPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                textoSelecionarAssalto = textoSelecionarAssaltoIngles;
                textoIniciarAssalto = textoIniciarAssaltoIngles;
                textoMissoes = textoMissoesIngles;
                break;
        }
    }
}
