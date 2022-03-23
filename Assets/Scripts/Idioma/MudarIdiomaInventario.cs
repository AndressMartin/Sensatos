using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MudarIdiomaInventario : MonoBehaviour
{
    //Componentes
    //Titulos
    [SerializeField] private TMP_Text tituloArmas;
    [SerializeField] private TMP_Text tituloAtalhos;
    [SerializeField] private TMP_Text tituloInventario;
    [SerializeField] private TMP_Text tituloMelhorias;
    [SerializeField] private TMP_Text tituloItensChave;
    [SerializeField] private TMP_Text tituloMissoes;
    [SerializeField] private TMP_Text tituloConquistas;

    //Painel de Escolha dos Itens
    [SerializeField] private TMP_Text textoUsar;
    [SerializeField] private TMP_Text textoAdicionarAosAtalhos;
    [SerializeField] private TMP_Text textoAlterarAPosicao;
    [SerializeField] private TMP_Text textoJogarFora;

    //Painel de Escolha dos Atalhos
    [SerializeField] private TMP_Text textoRemoverDosAtalhos;
    [SerializeField] private TMP_Text textoAlterarAPosicaoAtalho;

    //Painel de Confirmacao para Jogar um Item Fora
    [SerializeField] private TMP_Text textoPerguntaJogarFora;
    [SerializeField] private TMP_Text textoNao;
    [SerializeField] private TMP_Text textoSim;

    public void TrocarIdioma(MudarIdiomaUI.TextosUI textosUI, GeneralManagerScript generalManager)
    {
        this.tituloArmas.text = textosUI.tituloArmas;
        this.tituloAtalhos.text = textosUI.tituloAtalhos;
        this.tituloInventario.text = textosUI.tituloInventario;
        this.tituloMelhorias.text = textosUI.tituloMelhorias;
        this.tituloItensChave.text = textosUI.tituloItensChave;
        //this.tituloMissoes.text = textosUI.tituloMissoes;
        //this.tituloConquistas.text = textosUI.tituloConquistas;

        this.textoUsar.text = textosUI.textoUsar;
        this.textoAdicionarAosAtalhos.text = textosUI.textoAdicionarAosAtalhos;
        this.textoAlterarAPosicao.text = textosUI.textoAlterarAPosicao;
        this.textoJogarFora.text = textosUI.textoJogarFora;

        this.textoRemoverDosAtalhos.text = textosUI.textoRemoverDosAtalhos;
        this.textoAlterarAPosicaoAtalho.text = textosUI.textoAlterarAPosicaoAtalho;

        this.textoPerguntaJogarFora.text = textosUI.textoPerguntaJogarFora;
        this.textoNao.text = textosUI.textoNao;
        this.textoSim.text = textosUI.textoSim;

        generalManager.Hud.MenuDoInventario.MenuDosItensChave.SetNomeSemItens(textosUI.nomeSemItens);
        generalManager.Hud.MenuDoInventario.MenuDosItensChave.SetDescricaoSemItens(textosUI.descricaoSemItens);
    }
}
