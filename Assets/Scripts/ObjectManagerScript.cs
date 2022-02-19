using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerScript : MonoBehaviour
{
    //Listas
    [SerializeField] private List<ObjetoInteragivel> listaObjetosInteragiveis;
    [SerializeField] private List<ParedeModel> listaParedesQuebraveis;
    [SerializeField] private List<Enemy> listaInimigos;
    [SerializeField] private List<LockDown> listaAlarmes;
    [SerializeField] private List<Porta> listaPortas;

    //Getters
    [SerializeField] public List<ObjetoInteragivel> ListaObjetosInteragiveis => listaObjetosInteragiveis;
    [SerializeField] public List<ParedeModel> ListaParedesQuebraveis => listaParedesQuebraveis;
    [SerializeField] public List<Enemy> ListaInimigos => listaInimigos;
    [SerializeField] public List<LockDown> ListaAlarmes => listaAlarmes;
    [SerializeField] public List<Porta> ListaPortas => listaPortas;

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

    public void AdicionarAosAlarmes(LockDown alarme)
    {
        listaAlarmes.Add(alarme);
    }

    public void RemoverDosAlarmes(LockDown alarme)
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
}
