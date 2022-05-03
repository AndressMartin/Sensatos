using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteLoja : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private InventarioLoja inventarioLoja;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
    }

    public override void Interagir(Player player)
    {
        generalManager.Hud.MenuDaLoja.AbrirOMenuDaLoja(inventarioLoja);
    }
}
