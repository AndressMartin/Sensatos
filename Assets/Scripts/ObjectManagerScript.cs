using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerScript : MonoBehaviour
{
    //Listas
    [SerializeField] public List<ObjetoInteragivel> listaObjetosInteragiveis;
    [SerializeField] public List<ParedeModel> listaParedesQuebraveis;
    [SerializeField] public List<Enemy> listaInimigos;
    [SerializeField] public List<LockDown> listaAlarmes;
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
        if (!listaInimigos.Contains(inimigo))
        {
            listaInimigos.Add(inimigo);
        }
    }

    public void removerDosInimigos(Enemy inimigo)
    {
        listaInimigos.Remove(inimigo);
    }

    public void adicionarAosAlarmes(LockDown alarme)
    {
        listaAlarmes.Add(alarme);
    }

    public void removerDosAlarmes(LockDown alarme)
    {
        listaAlarmes.Remove(alarme);
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
