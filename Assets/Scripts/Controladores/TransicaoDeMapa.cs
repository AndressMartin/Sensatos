using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicaoDeMapa : MonoBehaviour
{
    //Managers
    protected GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] protected Vector3 posicaoPlayer;

    [SerializeField] protected CompositeCollider2D limiteDaCamera;

    protected virtual void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public virtual void FazerTransicao()
    {
        generalManager.Player.transform.position = posicaoPlayer;

        generalManager.CameraPrincipal.GetComponent<CinemachineConfiner>().m_BoundingShape2D = limiteDaCamera;
        generalManager.CameraPrincipal.transform.position = posicaoPlayer;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        generalManager.Hud.TelaTransicaoDeMapa.IniciarTransicao(this);
    }
}
