using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicaoDeMapa : MonoBehaviour
{
    //Managers
    protected GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] protected Transform posicaoPlayer;
    [SerializeField] protected EntityModel.Direcao direcao;

    [SerializeField] protected CompositeCollider2D limiteDaCamera;

    protected virtual void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Desativar sprite renderer
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public virtual void FazerTransicao()
    {
        generalManager.Player.transform.position = posicaoPlayer.position;
        generalManager.Player.ChangeDirection(direcao);

        generalManager.CameraPrincipal.GetComponent<CinemachineConfiner>().m_BoundingShape2D = limiteDaCamera;
        generalManager.CameraPrincipal.gameObject.transform.parent.transform.position = posicaoPlayer.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            generalManager.Hud.TelaTransicaoDeMapa.IniciarTransicao(this);
        }
    }
}
