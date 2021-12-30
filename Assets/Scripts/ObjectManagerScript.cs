using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerScript : MonoBehaviour
{
    [SerializeField] public List<ObjetoInteragivel> listaObjetosInteragiveis;
    [SerializeField] public List<ParedeModel> listaParedesQuebraveis;
    [SerializeField] public List<Enemy> listaInimigos;
    [SerializeField] public List<LockDown> listaAlarme;
    [SerializeField] public List<Porta> listaPortas;

    public void adicionarAosObjetosInteragiveis(ObjetoInteragivel objetoInteragivel)
    {
        listaObjetosInteragiveis.Add(objetoInteragivel);
    }

    public void removerDosObjetosInteragiveis(ObjetoInteragivel objetoInteragivel)
    {
        listaObjetosInteragiveis.Remove(objetoInteragivel);
    }

    public void adicionarAsParedesQuebraveis(ParedeModel paredeQuebravel)
    {
        listaParedesQuebraveis.Add(paredeQuebravel);
    }

    public void removerDasParedesQuebraveis(ParedeModel paredeQuebravel)
    {
        listaParedesQuebraveis.Remove(paredeQuebravel);
    }

    public void adicionarAosInimigos(Enemy inimigo)
    {
        listaInimigos.Add(inimigo);
    }

    public void removerDosInimigos(Enemy inimigo)
    {
        listaInimigos.Remove(inimigo);
    }

    public void adicionarAosAlarmes(LockDown alarme)
    {
        listaAlarme.Add(alarme);
    }

    public void removerDosAlarmes(LockDown alarme)
    {
        listaAlarme.Remove(alarme);
    }

    public void adicionarAsPortas(Porta porta)
    {
        listaPortas.Add(porta);
    }

    public void removerDasPortas(Porta porta)
    {
        listaPortas.Remove(porta);
    }
}
