using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IAEnemyLockdown : IAEnemy
{
    [SerializeField] Vector2 salaSeguranša;

    public override void Start()
    {
        Iniciar();
    }

    public override void Iniciar()
    {
        if (iniciado == true)
        {
            return;
        }

        base.Iniciar();
        salaSeguranša = transform.position;
    }

    protected override void StateMachine()
    {
        switch (estadoDeteccaoPlayer)
        {
            case EstadoDeteccaoPlayer.NaoToVendoPlayer:
                if(emLockDown)
                {
                    if(vendoPlayer)
                    {
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.PlayerDetectado;
                        verifiqueiUltimaPosicaoJogador = false;

                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.FazerRotinaLockdow;
                    }
                }
                else//voltar pro spawn
                {
                    inimigoEstados = InimigoEstados.Patrulhar;
                }
                break;
            case EstadoDeteccaoPlayer.PlayerDetectado:
                vendoPlayer = vendoPlayerCircular;
                if(vendoPlayer)
                {
                    posicaoUltimoLugarVisto = posicaoAtualPlayer;

                    if (playerAreaAtaque)
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
                    if (Contador(ref tempoEsquecerPlayer, tempoEsquecerPlayerMax)) //caso nao o veja chamar contador para perder o inimigo de vista
                    {
                        if (verifiqueiUltimaPosicaoJogador)
                        {
                            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.NaoToVendoPlayer;
                        }

                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
                    }
                }
                break;

        }
    }
    protected override void AndandoUltimaPosicaoPlayerConhecida()
    {
        base.AndandoUltimaPosicaoPlayerConhecida();

    }
    public override void ReceberLockDown(Vector2 _posicaoPlayer)
    {
        emLockDown = true;
        posicaoUltimoLugarVisto = _posicaoPlayer;
    }
    public override void DesativarLockDown()
    {
        emLockDown = false;
    }
    protected override void Patrulhar()
    {
        if (VerificarChegouAteAlvo(salaSeguranša))
        {
            gameObject.SetActive(false);
        }
    }

}
