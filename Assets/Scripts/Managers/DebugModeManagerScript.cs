using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugModeManagerScript : MonoBehaviour
{
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private DebugModeUIScript debugModeUI;
    private Player player;

    //Variaveis
    [SerializeField] private List<ArmaDeFogo> armasIniciais;
    [SerializeField] private List<RoupaDeCamuflagem> roupasIniciais;
    [SerializeField] private List<Item> itensIniciais;

    private float fps;
    private float tempoFPS;

    public SaveConfiguracoes.Configuracoes configuracoes = new SaveConfiguracoes.Configuracoes(0, 0, IdiomaManager.Idioma.Portugues);

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.TomarDano(0, 2, 0, new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        }

        //Ativar/Desativar o modo de combate
        if (Input.GetKeyDown(KeyCode.O))
        {
            player.SetModoDeCombate(!(player.ModoDeCombate));
        }

        //Fazer um checkpoint
        if (Input.GetKeyDown(KeyCode.G))
        {
            generalManager.RespawnManager.SetCheckpoint(player.transform.position, player.GetDirecao);
        }

        //Respawnar
        if (Input.GetKeyDown(KeyCode.H))
        {
            generalManager.RespawnManager.Respawn();
        }

        //Ativar Lockdown
        if (Input.GetKeyUp(KeyCode.L))
        {
            generalManager.LockDownManager.AtivarLockDown(player.transform.position);
        }

        //Desativar Lockdown
        if (Input.GetKeyUp(KeyCode.K))
        {
            generalManager.LockDownManager.DesativarLockDown();
        }

        //Carregar Configuracoes
        if (Input.GetKeyUp(KeyCode.Z))
        {
            configuracoes.volumeMusica = SaveConfiguracoes.configuracoes.volumeMusica;
            configuracoes.volumeEfeitosSonoros = SaveConfiguracoes.configuracoes.volumeEfeitosSonoros;
            configuracoes.idioma = SaveConfiguracoes.configuracoes.idioma;
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

        foreach (RoupaDeCamuflagem roupa in roupasIniciais)
        {
            player.Inventario.AddRoupa(roupa);
        }

        foreach (Item item in itensIniciais)
        {
            switch (item.Tipo)
            {
                case Item.TipoItem.Consumivel:
                    player.Inventario.AdicionarItem(item);
                    break;

                case Item.TipoItem.Ferramenta:
                    player.Inventario.AdicionarItem(item);
                    break;

                case Item.TipoItem.ItemChave:
                    player.InventarioMissao.AdicionarItem(item);
                    break;
            }
        }
    }
}
