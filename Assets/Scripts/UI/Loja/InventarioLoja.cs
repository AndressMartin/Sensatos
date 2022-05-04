using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listas/Inventario de Loja")]

public class InventarioLoja : ScriptableObject
{
    //Variaveis
    [SerializeField] private List<ArmaLoja> listaDeArmas;
    [SerializeField] private List<ItemLoja> listaDeItens;

    //Getters
    public List<ArmaLoja> ListaDeArmas => listaDeArmas;
    public List<ItemLoja> ListaDeItens => listaDeItens;

    [System.Serializable]
    public class ArmaLoja
    {
        //Enuns
        public enum TipoCompra { Arma, Municao }

        //Variaveis
        [SerializeField] private ArmaDeFogo arma;
        [SerializeField] private int precoArma;
        [SerializeField] private int precoMunicao;
        [SerializeField] private int[] precoMelhoria;
        private TipoCompra tipo;

        [SerializeField] private Flags.Flag flag;

        //Getters
        public ArmaDeFogo Arma => arma;
        public int PrecoArma => precoArma;
        public int PrecoMunicao => precoMunicao;
        public int[] PrecoMelhoria => precoMelhoria;
        public TipoCompra Tipo => tipo;
        public Flags.Flag Flag => flag;

        //Setters
        public void SetTipo(TipoCompra tipo)
        {
            this.tipo = tipo;
        }
    }

    [System.Serializable]
    public class ItemLoja
    {
        //Variaveis
        [SerializeField] private Item item;
        [SerializeField] private int preco;

        [SerializeField] private Flags.Flag flag;

        //Getters
        public Item Item => item;
        public int Preco => preco;
        public Flags.Flag Flag => flag;
    }
}
