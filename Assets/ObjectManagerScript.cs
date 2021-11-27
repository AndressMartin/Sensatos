using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManagerScript : MonoBehaviour
{
    [SerializeField] public List<ObjetoInteragivel> listaObjetosInteragiveis;
    [SerializeField] public List<Enemy> listaInimigos;
    //[SerializeField] public List<ItemColetavel> listaItensColetaveis;

    public void adicionarAosObjetosInteragiveis(ObjetoInteragivel objetoInteragivel)
    {
        listaObjetosInteragiveis.Add(objetoInteragivel);
    }

    public void adicionarAosInimigos(Enemy inimigo)
    {
        listaInimigos.Add(inimigo);
    }
}
