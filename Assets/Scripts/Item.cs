using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Tipo { Consumivel, Ferramenta, ItemChave };

    [SerializeField] public Tipo tipo;
    public Sprite spriteArma;
    [SerializeField] public string nome;
    public string descricao = "Não há descrição.";


    virtual public void Usar(Player player)
    {
        //Nada.
    }

    virtual public void UsarNaGameplay(Player player)
    {
        //Nada.
    }

    virtual public string GetNomeAnimacao()
    {
        return "";
    }

    public void ThrowAway()
    {

    }
}
