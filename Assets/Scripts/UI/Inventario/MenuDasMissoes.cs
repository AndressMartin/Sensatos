using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuDasMissoes : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private ListaSlot[] missoes;

    [SerializeField] private TMP_Text nomeDaMissao;
    [SerializeField] private TMP_Text descricaoDaMissao;

    [SerializeField] private RectTransform barraDeRolagem;

    [SerializeField] private Sprite imagemMissaoConcluida;
    [SerializeField] private Sprite imagemMissaoNaoConcluida;

    //Variaveis
    private int selecao;
    private int scrool;

    private float barraDeRolagemAlturaInicial;

    private List<ReferenciaMissao> listaDeMissoes = new List<ReferenciaMissao>();

    private string nomeSemMissoes = "";
    private string descricaoSemMissoes = "";

    [SerializeField] private string nomeSemMissoesPortugues;
    [SerializeField] private string descricaoSemMissoesPortugues;

    [SerializeField] private string nomeSemMissoesIngles;
    [SerializeField] private string descricaoSemMissoesIngles;

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

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        selecao = 0;
        scrool = 0;

        barraDeRolagemAlturaInicial = barraDeRolagem.sizeDelta.y;

        foreach (ListaSlot missao in missoes)
        {
            missao.Iniciar();
            missao.ZerarInformacoes();
            missao.SetCor(ListaSlot.Cor.Vermelho);
        }

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();

        iniciado = true;
    }

    private void AtualizarInformacoesSemMissao()
    {
        nomeDaMissao.text = nomeSemMissoes;
        descricaoDaMissao.text = descricaoSemMissoes;
    }

    private void AtualizarInformacoesDaMissao()
    {
        nomeDaMissao.text = listaDeMissoes[selecao].Missao.Nome;
        descricaoDaMissao.text = listaDeMissoes[selecao].Missao.Descricao;
    }

    private void AtualizarScroolDasMissoes()
    {
        for (int i = 0; i < missoes.Length; i++)
        {
            if (scrool + i >= listaDeMissoes.Count || scrool + i < 0)
            {
                missoes[i].gameObject.SetActive(false);
            }
            else
            {
                missoes[i].AtualizarNome(listaDeMissoes[scrool + i].Missao.Nome);

                if (listaDeMissoes[scrool + i].Missao.GetEstado == Missoes.Estado.Concluida)
                {
                    missoes[i].AtualizarImagem(imagemMissaoConcluida);
                }
                else
                {
                    missoes[i].AtualizarImagem(imagemMissaoNaoConcluida);
                }

                missoes[i].gameObject.SetActive(true);

                missoes[i].Selecionado(false);

                //Muda a cor do slot de acordo com o tipo da missao
                if (listaDeMissoes[scrool + i].TipoMissao == ReferenciaMissao.Tipo.Principal)
                {
                    missoes[i].SetCor(ListaSlot.Cor.Vermelho);
                }
                else
                {
                    missoes[i].SetCor(ListaSlot.Cor.Amarelho);
                }
            }
        }

        missoes[selecao - scrool].Selecionado(true);

        //Posicao da Barra de Rolagem
        if (missoes.Length < listaDeMissoes.Count)
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / listaDeMissoes.Count) * scrool * -1);
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

        //Criar a lista com as missoes principais e as secundarias que ja foram ativadas
        listaDeMissoes.Clear();
        listaDeMissoes = new List<ReferenciaMissao>();

        if(generalManager.AssaltoManager != null)
        {
            if (generalManager.AssaltoManager.GetAssaltoAtual != null)
            {
                foreach (Missao missao in generalManager.AssaltoManager.GetAssaltoAtual.GetMissoesPrincipais)
                {
                    listaDeMissoes.Add(new ReferenciaMissao(missao, ReferenciaMissao.Tipo.Principal));
                }

                foreach (Missao missao in generalManager.AssaltoManager.GetAssaltoAtual.GetMissoesSecundarias)
                {
                    if (missao.GetEstado != Missoes.Estado.Inativa)
                    {
                        listaDeMissoes.Add(new ReferenciaMissao(missao, ReferenciaMissao.Tipo.Secundaria));
                    }
                }
            }
        }

        //Tamanho da Barra de Rolagem
        if (missoes.Length < listaDeMissoes.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * ((float)missoes.Length / listaDeMissoes.Count));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }

        AtualizarScroolDasMissoes();

        if (listaDeMissoes.Count > 0)
        {
            AtualizarInformacoesDaMissao();
        }
        else
        {
            AtualizarInformacoesSemMissao();
        }

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    public void MenuMissoes()
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

                AtualizarScroolDasMissoes();
                AtualizarInformacoesDaMissao();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento1);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao < listaDeMissoes.Count - 1)
            {
                selecao++;

                if (selecao - scrool > missoes.Length - 1)
                {
                    scrool = selecao - (missoes.Length - 1);
                }

                AtualizarScroolDasMissoes();
                AtualizarInformacoesDaMissao();

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

    private void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                nomeSemMissoes = nomeSemMissoesPortugues;
                descricaoSemMissoes = descricaoSemMissoesPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                nomeSemMissoes = nomeSemMissoesIngles;
                descricaoSemMissoes = descricaoSemMissoesIngles;
                break;
        }
    }

    private struct ReferenciaMissao
    {
        //Enuns
        public enum Tipo { Principal, Secundaria}

        //Variaveis
        private Missao missao;
        private Tipo tipoMissao;

        //Getters
        public Missao Missao => missao;
        public Tipo TipoMissao => tipoMissao;

        public ReferenciaMissao(Missao missao, Tipo tipoMissao)
        {
            this.missao = missao;
            this.tipoMissao = tipoMissao;
        }
    }
}
