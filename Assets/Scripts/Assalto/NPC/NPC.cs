using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : ObjetoInteragivel
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    private bool iniciado = false;
    [SerializeField] private Missao missao;

    void Start()
    {
        Iniciar();
    }

    private void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        generalManager.ObjectManager.AdicionarAosObjetosInteragiveis(this);
    }

    public override void Interagir(Player player)
    {
        AssaltoManager.VerificarMissao(missao,player);
    }
}
