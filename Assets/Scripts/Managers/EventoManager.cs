using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoManager : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] protected Transform posicaoPlayerNovoJogo;
    [SerializeField] protected EntityModel.Direcao direcaoNovoJogo;

    [SerializeField] protected CompositeCollider2D limiteDaCameraNovoJogo;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        StartCoroutine(AjustarPosicaoDoJogador());
    }

    private IEnumerator AjustarPosicaoDoJogador()
    {
        yield return null;

        if(GameManager.instance.CapituloAtual == GameManager.Capitulo.Inicio)
        {
            Vector3 deltaPosition = posicaoPlayerNovoJogo.transform.position - generalManager.Player.transform.position;

            generalManager.Player.transform.position = posicaoPlayerNovoJogo.position;
            generalManager.Player.ChangeDirection(direcaoNovoJogo);

            generalManager.CameraPrincipal.GetComponent<CinemachineConfiner>().m_BoundingShape2D = limiteDaCameraNovoJogo;

            generalManager.CameraPrincipal.OnTargetObjectWarped(generalManager.Player.transform, deltaPosition);
        }
    }
}
