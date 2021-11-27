using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : ObjetoInteragivel
{
    public Chave obj;
    private GameObject player;
    private AnimacaoPorta animacao;

    private BoxCollider2D colisao;

    private ObjectManagerScript objectManager;

    bool aberto;
    public bool trancado;
    // Start is called before the first frame update
    void Start()
    {
        animacao = GetComponent<AnimacaoPorta>();

        colisao = GetComponent<BoxCollider2D>();

        //Se adicionar a lista de objetos interagiveis do ObjectManager
        objectManager = FindObjectOfType<ObjectManagerScript>();
        objectManager.adicionarAosObjetosInteragiveis(this);
    }


    public override void Interagir(Player player)
    {
        if (player.GetComponent<InventarioMissao>().itens.Contains(obj) && trancado)
        {
            //Debug.Log("destrancou porta");
            trancado = false;
            //spriteRenderer.color = (Color.blue);
        }

        if (!trancado)
        {
            //Debug.Log("abriur porta");
            aberto = true;
            if (animacao.GetAnimacaoAtual() != "Aberta")
            {
                animacao.TrocarAnimacao("Aberta");
            }
            //spriteRenderer.color = (Color.red);
            Door(aberto);
        }
    }

    public void FecharPorta()
    {
        if (!trancado && aberto)
        {
            aberto = false;
            if (animacao.GetAnimacaoAtual() != "Fechada")
            {
                animacao.TrocarAnimacao("Fechada");
            }
            Door(aberto);
        }
    }

    void Door(bool portaAberta)
    {
        colisao.isTrigger = portaAberta;
    }
}
