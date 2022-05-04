using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{
    //Variaveis
    private static bool[] flags;

    //Enuns
    public enum Flag { Teste1, Teste2, Teste3, Teste4, ItemT1Pego, ItemT2Pego, ItemT3Pego, ItemT4Pego, ItemT5Pego }

    //Getters
    public static bool GetFlag(Flag flag)
    {
        return flags[(int)flag];
    }

    public static bool[] GetFlagList => flags;

    //Setters
    public static void SetFlag(Flag flag, bool ativa)
    {
        flags[(int)flag] = ativa;
    }

    private void Awake()
    {
        //Inicia a lista de flags
        if (flags == null)
        {
            ResetarFlags();
        }
    }

    public static void ResetarFlags()
    {
        flags = new bool[System.Enum.GetValues(typeof(Flag)).Length];

        for (int i = 0; i < flags.Length; i++)
        {
            flags[i] = false;
        }
    }

    public static void PassarInformacoesSave(SaveData.SaveFile save)
    {
        for (int i = 0; i < flags.Length; i++)
        {
            if(i < save.flags.Length)
            {
                flags[i] = save.flags[i];
            }
            else
            {
                Debug.LogWarning("O lista de flags do save e menor que a do jogo!");
                break;
            }
        }
    }
}
