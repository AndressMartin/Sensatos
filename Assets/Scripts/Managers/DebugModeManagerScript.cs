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

    //Variaveis
    [SerializeField] private bool exibirInterfaceDeDebug;

    [SerializeField] private List<ArmaDeFogo> armasIniciais;
    [SerializeField] private List<RoupaDeCamuflagem> roupasIniciais;
    [SerializeField] private List<Item> itensIniciais;

    private float fps;
    private float tempoFPS;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Componentes
        debugModeUI = FindObjectOfType<DebugModeUIScript>();

        //Variaveis
        fps = 0;
        tempoFPS = 0;

        if(exibirInterfaceDeDebug == true)
        {
            debugModeUI.DebugModeUIAtiva(true);
        }

        StartCoroutine(SetarVariaveisDeTeste());
    }

    private void Update()
    {
        ComandosDeDebug();

        if(exibirInterfaceDeDebug == true)
        {
            ContadorDeFPS();
        }
    }

    private void ComandosDeDebug()
    {
        //Tomar dano
        if (Input.GetKeyDown(KeyCode.F))
        {
            generalManager.Player.TomarDano(10, 2, 0, new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        }

        //Ativar/Desativar o modo de combate
        if (Input.GetKeyDown(KeyCode.O))
        {
            generalManager.Player.SetModoDeCombate(!(generalManager.Player.ModoDeCombate));
        }

        //Fazer um checkpoint
        if (Input.GetKeyDown(KeyCode.G))
        {
            generalManager.RespawnManager.SetCheckpoint(generalManager.Player.transform.position, generalManager.Player.GetDirecao);
        }

        //Respawnar
        if (Input.GetKeyDown(KeyCode.H))
        {
            generalManager.RespawnManager.Respawn();
        }

        //Ativar Lockdown
        if (Input.GetKeyUp(KeyCode.L))
        {
            generalManager.LockDownManager.AtivarLockDown(generalManager.Player.transform.position);
        }

        //Desativar Lockdown
        if (Input.GetKeyUp(KeyCode.K))
        {
            generalManager.LockDownManager.DesativarLockDown();
        }

        //Salvar o jogo
        if (Input.GetKeyUp(KeyCode.V))
        {
            SaveManager.instance.SalvarJogo(1);
            Debug.Log("O jogo foi salvo no slot 1.");
        }

        //Carregar o jogo
        if (Input.GetKeyUp(KeyCode.B))
        {
            SaveManager.instance.CarregarJogo(1);
            Debug.Log("O save no slot 1 foi carregado.");
        }

        //Liberar mais assaltos
        if (Input.GetKeyUp(KeyCode.C))
        {
            GameManager.instance.SetAssaltosLiberados(GameManager.instance.AssaltosLiberados + 1);
        }

        //Liberar a compra das armas e itens
        if (Input.GetKeyUp(KeyCode.N))
        {
            generalManager.Player.Inventario.SetDinheiro(generalManager.Player.Inventario.Dinheiro + 50000);

            Flags.SetFlag(Flags.Flag.Teste1, true);
            Flags.SetFlag(Flags.Flag.Teste2, true);
            Flags.SetFlag(Flags.Flag.Teste3, true);
            Flags.SetFlag(Flags.Flag.Teste4, true);
        }

        //Bloquear a compra das armas e itens
        if (Input.GetKeyUp(KeyCode.M))
        {
            Flags.SetFlag(Flags.Flag.Teste1, false);
            Flags.SetFlag(Flags.Flag.Teste2, false);
            Flags.SetFlag(Flags.Flag.Teste3, false);
            Flags.SetFlag(Flags.Flag.Teste4, false);
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            generalManager.Player.Inventario.ArmaSlot[generalManager.Player.Inventario.ArmaAtual].SetNivelMelhoria(generalManager.Player.Inventario.ArmaSlot[generalManager.Player.Inventario.ArmaAtual].NivelMelhoria + 1);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            generalManager.Player.Inventario.ArmaSlot[generalManager.Player.Inventario.ArmaAtual].SetNivelMelhoria(generalManager.Player.Inventario.ArmaSlot[generalManager.Player.Inventario.ArmaAtual].NivelMelhoria + 1);
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
            generalManager.Player.Inventario.AdicionarArma(arma);
        }

        foreach (RoupaDeCamuflagem roupa in roupasIniciais)
        {
            generalManager.Player.Inventario.AdicionarRoupa(roupa);
        }

        foreach (Item item in itensIniciais)
        {
            switch (item.Tipo)
            {
                case Item.TipoItem.Consumivel:
                    generalManager.Player.Inventario.AdicionarItem(item);
                    break;

                case Item.TipoItem.Ferramenta:
                    generalManager.Player.Inventario.AdicionarItem(item);
                    break;

                case Item.TipoItem.ItemChave:
                    ItemChave itemChave = (ItemChave)item;
                    generalManager.Player.InventarioMissao.AdicionarItem(itemChave);
                    break;
            }
        }

        generalManager.Player.SetRespawn(generalManager.Player.transform.position, generalManager.Player.GetDirecao);

        generalManager.Hud.AtualizarPlayerHUD();
    }
}
