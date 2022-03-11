using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDasArmas : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Variaveis
    [SerializeField] private RectTransform scroolDasArmas;
    [SerializeField] private SelecaoArma[] armas;

    private int indiceArmaAtual;
    private int scroolID;

    private bool iniciado = false;

    void Start()
    {
        Iniciar();
    }

    public void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Variaveis
        indiceArmaAtual = 0;
        scroolID = 0;

        foreach(SelecaoArma selecaoArma in armas)
        {
            selecaoArma.Iniciar();
        }

        iniciado = true;
    }

    public void AtualizarPosicaoDoScroolDasArmas(float posY)
    {
        scroolDasArmas.transform.position = new Vector2(scroolDasArmas.transform.position.x, posY);
    }

    private void AtualizarInformacoesDasArmas()
    {
        armas[0].AtualizarInformacoes(generalManager.Player.Inventario.Armas[scroolID]);

        if (scroolID + 1 >= generalManager.Player.Inventario.Armas.Count)
        {
            armas[1].ZerarInformacoes();
        }
        else
        {
            armas[1].AtualizarInformacoes(generalManager.Player.Inventario.Armas[scroolID + 1]);
        }

        if (scroolID + 2 >= generalManager.Player.Inventario.Armas.Count)
        {
            armas[2].ZerarInformacoes();
        }
        else
        {
            armas[2].AtualizarInformacoes(generalManager.Player.Inventario.Armas[scroolID + 2]);
        }
    }

    public void IniciarScrool(int novoIndiceArmaAtual)
    {
        indiceArmaAtual = novoIndiceArmaAtual;

        for(int i = 0; i < generalManager.Player.Inventario.Armas.Count; i++)
        {
            if(generalManager.Player.Inventario.Armas[i] == generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual])
            {
                scroolID = i;
                break;
            }
        }

        armas[0].Selecionado(true);
        armas[1].Selecionado(false);
        armas[2].Selecionado(false);

        AtualizarInformacoesDasArmas();
    }

    public void SelecionandoArma()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(scroolID > 0)
            {
                scroolID--;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (scroolID < generalManager.Player.Inventario.Armas.Count - 1)
            {
                scroolID++;
            }
        }

        AtualizarInformacoesDasArmas();
    }

    public void ConfirmarArma()
    {
        int indiceComparacao;
        ArmaDeFogo armaTemp;

        if(indiceArmaAtual == 0)
        {
            indiceComparacao = 1;
        }
        else
        {
            indiceComparacao = 0;
        }

        //Confere se a arma que o jogador vai equipar e igual a outra arma que o jogador tem equipada, se for verdade, inverte as armas equipadas
        if(generalManager.Player.Inventario.Armas[scroolID] == generalManager.Player.Inventario.ArmaSlot[indiceComparacao])
        {
            armaTemp = generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual];

            generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual] = generalManager.Player.Inventario.Armas[scroolID];
            generalManager.Player.Inventario.ArmaSlot[indiceComparacao] = armaTemp;
        }
        else
        {
            generalManager.Player.Inventario.ArmaSlot[indiceArmaAtual] = generalManager.Player.Inventario.Armas[scroolID];
        }
    }
}
