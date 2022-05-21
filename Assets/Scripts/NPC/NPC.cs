using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    
    //Managers
    private GeneralManagerScript generalManager;

    //Componentes
    private BoxCollider2D colisao;

    NPCDialogo NpcDialogo;
    NpcMissao npcMissao;

    //Variaveis
    [SerializeField] LayerMask layerDasZonas;

    Assalto assaltoAtual;
    private Missao missaoAtual;
    private DialogueList listaDialogoAtual;
    [SerializeField] List<CapituloDialogoNpc> capituloDialogoNpc;

    private int zona;

    //Getters
    public Missao GetMissaoAtual => missaoAtual;
    public NpcMissao GetNpcMissao => npcMissao;
    public DialogueList GetDialogueListSemMissao => RetornarDialogoGenericoAtual().GetDialogueList;
    public GeneralManagerScript GetGeneralManager => generalManager;
    public int Zona => zona;

    void Start()
    {
        //Managers
        generalManager = FindObjectOfType<GeneralManagerScript>();

        //Se adicionar a lista de inimigos do ObjectManager
        generalManager.ObjectManager.AdicionarAosNPCs(this);

        //Componentes
        colisao = GetComponent<BoxCollider2D>();
        NpcDialogo = GetComponent<NPCDialogo>();
        npcMissao = GetComponent<NpcMissao>();

        NpcDialogo.Iniciar(this);

        foreach (var item in capituloDialogoNpc)
        {
            item.InicializarContador();
        }

        listaDialogoAtual = RetornarDialogoGenericoAtual().GetDialogueList;

        NpcDialogo.TrocarDialogoComponenteLista(listaDialogoAtual.GetDialogueList[0]);

        missaoAtual = null;

        SetarZona();
    }

    public void TrocarMissaoAtual(Missao _missao)
    {
        missaoAtual = _missao;
    }
    public void ReceberAssaltoDoManager(Assalto assalto)
    {
        if(assaltoAtual != assalto)
        {
            assaltoAtual = assalto;
            bool value = false;
            //Verifica se possui o assalto que recebeu tem alguma missao sua
            foreach (var item in assalto.GetMissoesPrincipais)
            {
                foreach (var item2 in npcMissao.GetListaMissao)
                {
                    if (item.GetId == item2.GetMissao.GetId)
                    {
                        value = true;
                    }
                }
            }
            foreach (var item in assalto.GetMissoesSecundarias)
            {
                foreach (var item2 in npcMissao.GetListaMissao)
                {
                    if (item.GetId == item2.GetMissao.GetId)
                    {
                        value = true;
                    }
                }
            }

            if (!value)
            {
                missaoAtual = null;
                listaDialogoAtual = RetornarDialogoGenericoAtual().GetDialogueList;
                NpcDialogo.TrocarDialogoComponenteLista(listaDialogoAtual.GetDialogueList[0]);
            }

            else
            {
                listaDialogoAtual = null;
                //tenta primeiro com missao principal, se npc n tiver missao principal desse assalto vai pra secundaria
                NpcDialogo.TrocarDialogoConformeMissao(assalto.GetMissoesPrincipais);
                if (listaDialogoAtual == null)
                {
                    NpcDialogo.TrocarDialogoConformeMissao(assalto.GetMissoesSecundarias);
                }
            }
        }
        
        else
        {
            print("São iguaos");
        }
    }

    /// <summary>
    /// Verifica o estado da missao e atualiza o dialogo do Npc
    /// </summary>
    /// <param name="player"></param>
    public void Interagir(Player player)
    {
        if (missaoAtual != null)
        {
            if (missaoAtual.GetEstado == Missoes.Estado.Concluida)
            {
                listaDialogoAtual = RetornarDialogoGenericoAtual().GetDialogueList;
                NpcDialogo.TrocarDialogoComponenteLista(listaDialogoAtual.GetDialogueList[0]);
            }
            else if (missaoAtual.GetEstado == Missoes.Estado.Ativa)
            {
                VerificarAssaltoMissao.VerificarMissao(missaoAtual, generalManager);
                NpcDialogo.TrocarDialogoMissaoEspecifico(missaoAtual, missaoAtual.GetEstado);

            }
            else if (missaoAtual.GetEstado == Missoes.Estado.Inativa)
            {
                VerificarAssaltoMissao.VerificarMissao(missaoAtual, generalManager);
                NpcDialogo.TrocarDialogoMissaoEspecifico(missaoAtual, Missoes.Estado.Ativa);
            }
        }
    }

    public void CompletarMissao()
    {
        missaoAtual.SetEstado(Missoes.Estado.Concluida);
    }

    public CapituloDialogoNpc RetornarDialogoGenericoAtual()
    {
        foreach (var item in capituloDialogoNpc)
        {
            if (item.GetCapitulo == GameManager.instance.CapituloAtual)
            {
                return item;
            }
        }
        return null;
    }

    private void SetarZona()
    {
        zona = 0;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(colisao.bounds.center, colisao.bounds.extents, 0, layerDasZonas);
        foreach (Collider2D objeto in hitColliders)
        {
            if (objeto.GetComponent<Zona>())
            {
                zona = objeto.GetComponent<Zona>().GetZona;
                break;
            }
        }
    }
}

[System.Serializable]
public class CapituloDialogoNpc
{
    [SerializeField]private GameManager.Capitulo capitulo;
    [SerializeField]private DialogueList dialogueList;

    private int cont = 0;
    private int contMax;

    public GameManager.Capitulo GetCapitulo => capitulo;
    public DialogueList GetDialogueList => dialogueList;
    public int GetCont => cont;
    public void InicializarContador()
    {
        contMax = dialogueList.GetDialogueList.Count;
    }
    public void addCont()
    {
        cont++;
        if (cont >= contMax)
            cont = 0;
    }
}
