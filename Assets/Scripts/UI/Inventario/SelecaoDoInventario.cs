using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelecaoDoInventario : MonoBehaviour
{
    [SerializeField] protected SelecaoStruct selecao;

    public SelecaoStruct Selecao => selecao;

    public abstract void Confirmar(MenuDoInventario menuDoInventario);
    public abstract void Voltar(MenuDoInventario menuDoInventario);
    public abstract void Selecionado(bool selecionado);

    [System.Serializable]
    public struct SelecaoStruct
    {
        [SerializeField] private SelecaoDoInventario cima;
        [SerializeField] private SelecaoDoInventario baixo;
        [SerializeField] private SelecaoDoInventario esquerda;
        [SerializeField] private SelecaoDoInventario direita;

        public SelecaoDoInventario Cima => cima;
        public SelecaoDoInventario Baixo => baixo;
        public SelecaoDoInventario Esquerda => esquerda;
        public SelecaoDoInventario Direita => direita;
    }
}
