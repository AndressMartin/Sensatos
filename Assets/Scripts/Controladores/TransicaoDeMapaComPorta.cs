using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicaoDeMapaComPorta : TransicaoDeMapa
{
    private PortaDeTransicao portaDeTransicao;

    protected override void Start()
    {
        base.Start();

        portaDeTransicao = GetComponent<PortaDeTransicao>();
    }

    public override void FazerTransicao()
    {
        base.FazerTransicao();

        portaDeTransicao.FecharPorta();
    }

    public void IniciarTransicao()
    {
        generalManager.Hud.TelaTransicaoDeMapa.IniciarTransicao(this);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //Nada
    }
}
