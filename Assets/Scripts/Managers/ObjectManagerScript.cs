using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerScript : MonoBehaviour
{
    //Listas
    [SerializeField] private List<ObjetoInteragivel> listaObjetosInteragiveis;
    [SerializeField] private List<ParedeModel> listaParedesQuebraveis;
    [SerializeField] private List<Enemy> listaInimigos;
    [SerializeField] private List<LockDownButton> listaAlarmes;
    [SerializeField] private List<Porta> listaPortas;
    [SerializeField] private List<CameraDeSeguranca> listaDeCamerasLockdown;

    [SerializeField] private List<NPC> listaDeNPCs;

    //Getters
    public List<ObjetoInteragivel> ListaObjetosInteragiveis => listaObjetosInteragiveis;
    public List<ParedeModel> ListaParedesQuebraveis => listaParedesQuebraveis;
    public List<Enemy> ListaInimigos => listaInimigos;
    public List<LockDownButton> ListaAlarmes => listaAlarmes;
    public List<Porta> ListaPortas => listaPortas;
    public List<CameraDeSeguranca> ListaDeCamerasLockdown => listaDeCamerasLockdown;
    public List<NPC> ListaDeNPCs => listaDeNPCs;

    public void AdicionarAosObjetosInteragiveis(ObjetoInteragivel objetoInteragivel)
    {
        listaObjetosInteragiveis.Add(objetoInteragivel);
    }

    public void RemoverDosObjetosInteragiveis(ObjetoInteragivel objetoInteragivel)
    {
        listaObjetosInteragiveis.Remove(objetoInteragivel);
    }

    public void AdicionarAsParedesQuebraveis(ParedeModel paredeQuebravel)
    {
        listaParedesQuebraveis.Add(paredeQuebravel);
    }

    public void RemoverDasParedesQuebraveis(ParedeModel paredeQuebravel)
    {
        listaParedesQuebraveis.Remove(paredeQuebravel);
    }

    public void AdicionarAosInimigos(Enemy inimigo)
    {
        if (!listaInimigos.Contains(inimigo))
        {
            listaInimigos.Add(inimigo);
        }
    }

    public void RemoverDosInimigos(Enemy inimigo)
    {
        listaInimigos.Remove(inimigo);
    }

    public void AdicionarAosAlarmes(LockDownButton alarme)
    {
        listaAlarmes.Add(alarme);
    }

    public void RemoverDosAlarmes(LockDownButton alarme)
    {
        listaAlarmes.Remove(alarme);
    }

    public void AdicionarAsPortas(Porta porta)
    {
        listaPortas.Add(porta);
    }

    public void RemoverDasPortas(Porta porta)
    {
        listaPortas.Remove(porta);
    }

    public void AdicionarAsCameras(CameraDeSeguranca camera)
    {
        listaDeCamerasLockdown.Add(camera);
    }

    public void RemoverDasCameras(CameraDeSeguranca camera)
    {
        listaDeCamerasLockdown.Remove(camera);
    }

    public void AdicionarAosNPCs(NPC npc)
    {
        listaDeNPCs.Add(npc);
    }

    public void RemoverDosNPCs(NPC npc)
    {
        listaDeNPCs.Remove(npc);
    }
}
