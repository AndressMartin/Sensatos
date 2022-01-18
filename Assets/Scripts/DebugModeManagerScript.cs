using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugModeManagerScript : MonoBehaviour
{
    //Managers
    private RespawnManagerScript respawnManager;

    //Componentes
    private DebugModeUIScript debugModeUI;
    private Player player;

    //Variaveis
    [SerializeField] private List<ArmaDeFogo> armasIniciais;
    [SerializeField] private List<Item> itensIniciais;

    private float fps;
    private float tempoFPS;

    void Start()
    {
        //Managers
        respawnManager = FindObjectOfType<RespawnManagerScript>();

        //Componentes
        debugModeUI = FindObjectOfType<DebugModeUIScript>();
        player = FindObjectOfType<Player>();

        //Variaveis
        fps = 0;
        tempoFPS = 0;

        debugModeUI.DebugModeUIAtiva(true);

        StartCoroutine(SetarVariaveisDeTeste());
    }

    private void Update()
    {
        ComandosDeDebug();
        ContadorDeFPS();
    }

    private void ComandosDeDebug()
    {
        //Tomar dano
        if (Input.GetKeyDown(KeyCode.G))
        {
            player.TomarDano(0, 2, 0, new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        }

        //Ativar/Desativar o modo de combate
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.SetModoDeCombate(!(player.ModoDeCombate));
        }

        //Fazer um checkpoint
        if (Input.GetKeyDown(KeyCode.H))
        {
            respawnManager.SetCheckpoint(player.transform.position, player.GetDirecao);
        }

        //Respawnar
        if (Input.GetKeyDown(KeyCode.J))
        {
            respawnManager.Respawn();
        }
    }

    private void ContadorDeFPS()
    {
        fps++;
        tempoFPS += Time.unscaledDeltaTime;

        if(tempoFPS >= 1)
        {
            debugModeUI.AtualizarTextoFPS(fps);

            fps = 0;
            tempoFPS -= 1;
        }
    }

    private IEnumerator SetarVariaveisDeTeste()
    {
        yield return null;

        foreach(ArmaDeFogo arma in armasIniciais)
        {
            player.Inventario.AddArma(arma);

            ArmaDeFogo novaArma = player.Inventario.Armas[player.Inventario.Armas.Count - 1];
            novaArma.AdicionarMunicao(novaArma.GetStatus.MunicaoMax + novaArma.GetStatus.MunicaoMaxCartucho);
        }

        foreach (Item item in itensIniciais)
        {
            switch (item.tipo)
            {
                case Item.Tipo.Consumivel:
                    player.Inventario.AddItem(item);
                    break;

                case Item.Tipo.Ferramenta:
                    player.Inventario.AddItem(item);
                    break;

                case Item.Tipo.ItemChave:
                    player.InventarioMissao.Add(item);
                    break;
            }
        }
    }
}
