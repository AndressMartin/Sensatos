using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Listas/Inventario de Loja")]

public class InventarioLoja : ScriptableObject
{
    //Variaveis
    [SerializeField] private List<ArmaLoja> listaDeArmas;
    [SerializeField] private List<ItemLoja> listaDeItens;

    private bool iniciado = false;

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
        public void IniciarArma()
        {
            ArmaDeFogo novaArma = ScriptableObject.Instantiate(arma);
            novaArma.name = arma.name;
            this.arma = novaArma;
        }

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

        //Setters
        public void IniciarItem()
        {
            Item novoItem = ScriptableObject.Instantiate(item);
            novoItem.name = item.name;
            this.item = novoItem;
        }
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        foreach (ArmaLoja armaLoja in listaDeArmas)
        {
            armaLoja.IniciarArma();
        }

        foreach (ItemLoja itemLoja in listaDeItens)
        {
            itemLoja.IniciarItem();
        }

        iniciado = true;
    }
}
