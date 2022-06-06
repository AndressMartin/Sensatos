using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LojaDeMelhorias : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private ItemLojaSlot[] melhorias;

    [SerializeField] private TMP_Text nomeDaMelhoria;
    [SerializeField] private TMP_Text descricaoDaMelhoria;

    [SerializeField] private TMP_Text dinheiroJogador;

    [SerializeField] private RectTransform barraDeRolagem;

    //Enums
    public enum Menu { Inicio }

    //Variaveis
    private int selecao;
    private int scrool;

    private Menu menuAtual;

    private float barraDeRolagemAlturaInicial;

    private string nomeSemMelhorias = "";
    private string descricaoSemMelhorias = "";
    private string textoMelhoria = "";

    [SerializeField] private string nomeSemMelhoriasPortugues;
    [SerializeField] private string descricaoSemMelhoriasPortugues;
    [SerializeField] private string textoMelhoriaPortugues;

    [SerializeField] private string nomeSemMelhoriasIngles;
    [SerializeField] private string descricaoSemMelhoriasIngles;
    [SerializeField] private string textoMelhoriaIngles;


    private bool iniciado = false;

    private List<MelhoriaLoja> listaDeMelhorias = new List<MelhoriaLoja>();

    //Variaveis Fixas
    private readonly int numeroDeColunas = 3;

    //Setters
    public void SetMenuAtual(Menu menuAtual)
    {
        this.menuAtual = menuAtual;
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

        //Adicionar a funcao de trocar idioma ao evento do Idioma Manager
        generalManager.IdiomaManager.EventoTrocarIdioma.AddListener(TrocarIdioma);

        //Variaveis
        selecao = 0;
        scrool = 0;

        barraDeRolagemAlturaInicial = barraDeRolagem.sizeDelta.y;

        foreach (ItemLojaSlot melhoriaSlot in melhorias)
        {
            melhoriaSlot.Iniciar();
            melhoriaSlot.ZerarInformacoes();
        }

        //Trocar o idioma uma vez para iniciar o objeto com o idioma correto
        TrocarIdioma();

        iniciado = true;
    }

    private void AtualizarInformacoesSemMelhorias()
    {
        nomeDaMelhoria.text = nomeSemMelhorias;
        descricaoDaMelhoria.text = descricaoSemMelhorias;

        dinheiroJogador.text = generalManager.Player.Inventario.Dinheiro.ToString();
    }

    private void AtualizarInformacoesDaMelhoria()
    {
        nomeDaMelhoria.text = listaDeMelhorias[selecao].Melhoria.Nome;
        descricaoDaMelhoria.text = listaDeMelhorias[selecao].Melhoria.Descricao + "\n\n" + textoMelhoria + " " + listaDeMelhorias[selecao].Arma.Nome + ".";

        dinheiroJogador.text = generalManager.Player.Inventario.Dinheiro.ToString();
    }

    private void AtualizarScroolDasMelhorias()
    {
        for (int i = 0; i < melhorias.Length; i++)
        {
            if (scrool + i >= listaDeMelhorias.Count || scrool + i < 0)
            {
                melhorias[i].gameObject.SetActive(false);
            }
            else
            {
                melhorias[i].AtualizarInformacoes(listaDeMelhorias[scrool + i].Melhoria.ImagemMelhoria, listaDeMelhorias[scrool + i].Preco);

                melhorias[i].gameObject.SetActive(true);
            }
        }

        foreach (ItemLojaSlot melhoriaSlot in melhorias)
        {
            melhoriaSlot.Selecionado(false);
        }

        melhorias[selecao - scrool].Selecionado(true);

        //Posicao da Barra de Rolagem
        if (melhorias.Length < listaDeMelhorias.Count)
        {
            barraDeRolagem.anchoredPosition = new Vector2(0, (barraDeRolagemAlturaInicial / Mathf.Ceil((float)listaDeMelhorias.Count / numeroDeColunas)) * (scrool / numeroDeColunas) * -1);
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

        SetMenuAtual(Menu.Inicio);

        AtualizarListaDeMelhorias();

        //Tamanho da Barra de Rolagem
        if (melhorias.Length < listaDeMelhorias.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * (Mathf.Ceil((float)melhorias.Length / numeroDeColunas) / Mathf.Ceil((float)listaDeMelhorias.Count / numeroDeColunas)));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }

        AtualizarScroolDasMelhorias();

        if (listaDeMelhorias.Count > 0)
        {
            AtualizarInformacoesDaMelhoria();
        }
        else
        {
            AtualizarInformacoesSemMelhorias();
        }

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Confirmar);
    }

    private void AtualizarListaDeMelhorias()
    {
        //Cria a lista com as melhorias disponiveis para comprar
        listaDeMelhorias.Clear();
        listaDeMelhorias = new List<MelhoriaLoja>();

        if (generalManager.Hud.MenuDaLoja.InventarioLoja != null)
        {
            if (generalManager.Hud.MenuDaLoja.InventarioLoja.ListaDeArmas.Count > 0)
            {
                foreach (InventarioLoja.ArmaLoja arma in generalManager.Hud.MenuDaLoja.InventarioLoja.ListaDeArmas)
                {
                    if (generalManager.Player.Inventario.PossuiArma(arma.Arma) == true)
                    {
                        ArmaDeFogo armaDoPlayer;
                        armaDoPlayer = generalManager.Player.Inventario.GetArma(arma.Arma);

                        if (armaDoPlayer.Melhorias.Count > 0)
                        {
                            if(armaDoPlayer.NivelMelhoria < armaDoPlayer.Melhorias.Count)
                            {
                                listaDeMelhorias.Add(new MelhoriaLoja(armaDoPlayer.Melhorias[armaDoPlayer.NivelMelhoria], arma.Arma, arma.PrecoMelhoria[armaDoPlayer.NivelMelhoria]));
                            }
                        }
                    }
                }
            }
        }

        //Tamanho da Barra de Rolagem
        if (melhorias.Length < listaDeMelhorias.Count)
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial * (Mathf.Ceil((float)melhorias.Length / numeroDeColunas) / Mathf.Ceil((float)listaDeMelhorias.Count / numeroDeColunas)));
        }
        else
        {
            barraDeRolagem.sizeDelta = new Vector2(barraDeRolagem.sizeDelta.x, barraDeRolagemAlturaInicial);
        }
    }

    private void ComprarMelhoria()
    {
        ArmaDeFogo arma;
        arma = generalManager.Player.Inventario.GetArma(listaDeMelhorias[selecao].Arma);

        arma.SetNivelMelhoria(arma.NivelMelhoria + 1);
        generalManager.Player.Inventario.SetDinheiro(generalManager.Player.Inventario.Dinheiro - (listaDeMelhorias[selecao].Preco));

        AtualizarListaDeMelhorias();

        //Ajusta a selecao e o scrool da lista
        if (selecao > listaDeMelhorias.Count - 1)
        {
            selecao = listaDeMelhorias.Count - 1;

            if (selecao < 0)
            {
                selecao = 0;
            }

            while (selecao < scrool)
            {
                scrool -= numeroDeColunas;
            }
        }

        //Ajusta o scrool caso a nova lista tenha uma linha a menos
        while (listaDeMelhorias.Count - scrool < melhorias.Length - (numeroDeColunas - 1))
        {
            if (scrool <= 0)
            {
                break;
            }

            scrool -= numeroDeColunas;
        }

        AtualizarScroolDasMelhorias();

        if (listaDeMelhorias.Count > 0)
        {
            AtualizarInformacoesDaMelhoria();
        }
        else
        {
            AtualizarInformacoesSemMelhorias();
        }

        generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Comprar);
    }

    public void MenuLojaDeMelhorias()
    {
        //Executa as funcoes do menu atual
        switch (menuAtual)
        {
            case Menu.Inicio:
                MenuInicial();
                break;
        }
    }

    private void MenuInicial()
    {
        //Mover para cima
        if (InputManager.Cima())
        {
            if (selecao - numeroDeColunas >= 0)
            {
                selecao -= numeroDeColunas;

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasMelhorias();
                AtualizarInformacoesDaMelhoria();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
            else if (selecao > 0)
            {
                selecao = 0;

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasMelhorias();
                AtualizarInformacoesDaMelhoria();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para baixo
        if (InputManager.Baixo())
        {
            if (selecao + numeroDeColunas < listaDeMelhorias.Count)
            {
                selecao += numeroDeColunas;

                if (selecao - scrool > melhorias.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasMelhorias();
                AtualizarInformacoesDaMelhoria();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
            else if (selecao < listaDeMelhorias.Count - 1)
            {
                selecao = listaDeMelhorias.Count - 1;

                if (selecao - scrool > melhorias.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasMelhorias();
                AtualizarInformacoesDaMelhoria();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a esquerda
        if (InputManager.Esquerda())
        {
            if (selecao > 0)
            {
                selecao--;

                if (selecao < scrool)
                {
                    scrool -= numeroDeColunas;
                }

                AtualizarScroolDasMelhorias();
                AtualizarInformacoesDaMelhoria();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Mover para a direita
        if (InputManager.Direita())
        {
            if (selecao < listaDeMelhorias.Count - 1)
            {
                selecao++;


                if (selecao - scrool > melhorias.Length - 1)
                {
                    scrool += numeroDeColunas;
                }

                AtualizarScroolDasMelhorias();
                AtualizarInformacoesDaMelhoria();

                generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Movimento2);
            }
        }

        //Voltar
        if (InputManager.Voltar())
        {
            generalManager.Hud.MenuDaLoja.SetMenuAtual(MenuDaLoja.Menu.Inicio);

            generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Voltar);
        }

        //Confirmar
        if (InputManager.Confirmar())
        {
            if(listaDeMelhorias.Count > 0)
            {
                if (generalManager.Player.Inventario.Dinheiro >= listaDeMelhorias[selecao].Preco)
                {
                    ComprarMelhoria();
                }
                else
                {
                    generalManager.Hud.SonsDeMenus.TocarSom(SonsDeMenus.Som.Falha);
                }
            }
        }
    }

    private void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                nomeSemMelhorias = nomeSemMelhoriasPortugues;
                descricaoSemMelhorias = descricaoSemMelhoriasPortugues;
                textoMelhoria = textoMelhoriaPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                nomeSemMelhorias = nomeSemMelhoriasIngles;
                descricaoSemMelhorias = descricaoSemMelhoriasIngles;
                textoMelhoria = textoMelhoriaIngles;
                break;
        }
    }

    public struct MelhoriaLoja
    {
        //Variaveis
        private ArmaDeFogo.Melhoria melhoria;
        private ArmaDeFogo arma;
        private int preco;

        //Getters
        public ArmaDeFogo.Melhoria Melhoria => melhoria;
        public ArmaDeFogo Arma => arma;
        public int Preco => preco;

        public MelhoriaLoja(ArmaDeFogo.Melhoria melhoria, ArmaDeFogo arma, int preco)
        {
            this.melhoria = melhoria;
            this.arma = arma;
            this.preco = preco;
        }
    }
}
