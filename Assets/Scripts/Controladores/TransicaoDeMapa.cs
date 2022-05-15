using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransicaoDeMapa : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private Vector3 posicaoPlayer;

    [SerializeField] private CompositeCollider2D limiteDaCamera;

    private void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();
    }

    public void FazerTransicao()
    {
        generalManager.Player.transform.position = posicaoPlayer;

        generalManager.CameraPrincipal.GetComponent<CinemachineConfiner>().m_BoundingShape2D = limiteDaCamera;
        generalManager.CameraPrincipal.transform.position = posicaoPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        generalManager.Hud.TelaTransicaoDeMapa.IniciarTransicao(this);
    }
}
