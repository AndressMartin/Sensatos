using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciadorDoPlayer : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private int vidaInicial;

    [SerializeField] private List<ArmaDeFogo> armasIniciais;
    [SerializeField] private List<RoupaDeCamuflagem> roupasIniciais;
    [SerializeField] private List<Item> itensIniciais;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        StartCoroutine(SetarVariaveisCorrotina());
    }

    public void SetarVariaveis()
    {
        generalManager.Player.SetVidaMaxima(vidaInicial);

        foreach (ArmaDeFogo arma in armasIniciais)
        {
            generalManager.Player.Inventario.AdicionarArma(arma);
        }

        foreach (RoupaDeCamuflagem roupa in roupasIniciais)
        {
            generalManager.Player.Inventario.AdicionarRoupa(roupa);
        }

        foreach (Item item in itensIniciais)
        {
            switch (item.Tipo)
            {
                case Item.TipoItem.Consumivel:
                    generalManager.Player.Inventario.AdicionarItem(item);
                    break;

                case Item.TipoItem.Ferramenta:
                    generalManager.Player.Inventario.AdicionarItem(item);
                    break;

                case Item.TipoItem.ItemChave:
                    ItemChave itemChave = (ItemChave)item;
                    generalManager.Player.InventarioMissao.AdicionarItem(itemChave);
                    break;
            }
        }

        generalManager.Player.SetRespawn(generalManager.Player.transform.position, generalManager.Player.GetDirecao);
    }

    private IEnumerator SetarVariaveisCorrotina()
    {
        yield return null;

        SetarVariaveis();
    }
}
