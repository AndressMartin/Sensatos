using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Enemy_inmigoDeLockdown : IA_Enemy_Basico
{
    protected override void StateMachine()
    {
        switch (estadoDeteccaoPlayer)
        {
            case EstadoDeteccaoPlayer.NaoToVendoPlayer:
                if(emLockDown)
                {
                    if(vendoPlayer)
                    {
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.Patrulhar;
                    }
                }
                else
                {
                    inimigoEstados = InimigoEstados.FazerRotinaLockdow;
                }
                break;
            case EstadoDeteccaoPlayer.playerDetectado:
                if(vendoPlayer)
                {
                    if(playerAreaAtaque)
                    {
                        inimigoEstados = InimigoEstados.AtacarPlayer;
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.AndandoAtePlayer;
                    }
                }
                else
                {
                    estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                }
                break;

        }
    }
    protected override void Patrulhar()
    {
        base.Patrulhar();
    }
    protected override void FazerRotinaLockdown()
    {
        base.FazerRotinaLockdown();
    }
   
}
