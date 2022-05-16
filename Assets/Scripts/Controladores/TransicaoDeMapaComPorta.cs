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
        generalManager.Player.transform.position = posicaoPlayer;

        generalManager.CameraPrincipal.GetComponent<CinemachineConfiner>().m_BoundingShape2D = limiteDaCamera;
        generalManager.CameraPrincipal.transform.position = posicaoPlayer;

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
