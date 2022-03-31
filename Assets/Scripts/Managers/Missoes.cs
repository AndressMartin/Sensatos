using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missoes : MonoBehaviour
{
    //Variaveis
    private static Missao.Estado[] estadosDasMissoes;

    //Getters
    public static Missao.Estado GetEstadoMissao(int indice)
    {
        return estadosDasMissoes[indice];
    }

    public static Missao.Estado[] GetEstadoMissaoList => estadosDasMissoes;

    //Setters
    public static void SetEstadoMissao(int indice, Missao.Estado estado)
    {
        estadosDasMissoes[indice] = estado;
    }

    private void Start()
    {
        //Inicia a lista de flags
        if (estadosDasMissoes == null)
        {
            ResetarEstadosDasMissoes();
        }
    }

    public static void ResetarEstadosDasMissoes()
    {
        estadosDasMissoes = new Missao.Estado[Listas.instance.ListaDeMissoes.TamanhoListaDeMissoes];

        for (int i = 0; i < estadosDasMissoes.Length; i++)
        {
            estadosDasMissoes[i] = Missao.Estado.Inativa;
        }
    }

    public static void PassarInformacoesSave(SaveData.SaveFile save)
    {
        for (int i = 0; i < estadosDasMissoes.Length; i++)
        {
            if (i < save.estadoDasMissoes.Length)
            {
                estadosDasMissoes[i] = save.estadoDasMissoes[i];
            }
            else
            {
                Debug.LogWarning("O lista de estados das missoes do save e menor que a do jogo!");
                break;
            }
        }
    }
}
