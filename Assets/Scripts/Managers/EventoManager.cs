using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoManager : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    [SerializeField] private DialogueActivator dialogueActivator;

    //Variaveis
    [SerializeField] private Transform posicaoPlayerNovoJogo;
    [SerializeField] private EntityModel.Direcao direcaoNovoJogo;
    [SerializeField] private CompositeCollider2D limiteDaCameraNovoJogo;

    [SerializeField] private int dinheiroParaCompletarOJogo;

    [SerializeField] private Transform posicaoPlayerFimDoJogo;
    [SerializeField] private EntityModel.Direcao direcaoFimDoJogo;
    [SerializeField] private CompositeCollider2D limiteDaCameraFimDoJogo;

    [SerializeField] private GameObject NPCsNoFimDoJogo;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        dialogueActivator = GetComponent<DialogueActivator>();

        NPCsNoFimDoJogo.SetActive(false);

        StartCoroutine(AjustarPosicaoDoJogador());
    }

    public bool CondicaoFimDoJogo()
    {
        return generalManager.Player.Inventario.Dinheiro >= dinheiroParaCompletarOJogo;
    }

    public void FimDoJogo()
    {
        SaveManager.instance.AutoSave();

        generalManager.Hud.TelaFimDoJogo.IniciarFimDoJogo();

        generalManager.Player.GetComponent<PlayerInput>().enabled = false;
    }

    public void PosicaoFimDoJogo()
    {
        NPCsNoFimDoJogo.SetActive(true);
        TeleportarJogador(posicaoPlayerFimDoJogo, direcaoFimDoJogo, limiteDaCameraFimDoJogo);
    }

    private void TeleportarJogador(Transform posicao, EntityModel.Direcao direcao, CompositeCollider2D limiteDaCamera)
    {
        Vector3 deltaPosition = posicao.position - generalManager.Player.transform.position;

        generalManager.Player.transform.position = posicao.position;
        generalManager.Player.ChangeDirection(direcao);

        generalManager.CameraPrincipal.GetComponent<CinemachineConfiner>().m_BoundingShape2D = limiteDaCamera;

        generalManager.CameraPrincipal.OnTargetObjectWarped(generalManager.Player.transform, deltaPosition);
    }

    public void MostrarDialogoFimDoJogo()
    {
        dialogueActivator.ShowDialogue(generalManager);
    }

    public void MostrarCreditos()
    {
        generalManager.Hud.TelaFimDoJogo.IniciarCreditos();
    }

    private IEnumerator AjustarPosicaoDoJogador()
    {
        yield return null;

        if(GameManager.instance.CapituloAtual == GameManager.Capitulo.Inicio)
        {
            TeleportarJogador(posicaoPlayerNovoJogo, direcaoNovoJogo, limiteDaCameraNovoJogo);
        }
    }
}
