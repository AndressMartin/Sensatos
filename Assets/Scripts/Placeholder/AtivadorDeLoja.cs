using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivadorDeLoja : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private InventarioLoja inventarioLoja;
    private InventarioLoja inventarioLojaRuntime;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);

        //Fazer uma copia do inventario da loja para usar no jogo
        InventarioLoja novaLoja = ScriptableObject.Instantiate(inventarioLoja);
        novaLoja.name = inventarioLoja.name;
        inventarioLojaRuntime = novaLoja;
    }

    public override void Interagir(Player player)
    {
        generalManager.Hud.MenuDaLoja.AbrirOMenuDaLoja(inventarioLojaRuntime);
    }
}
