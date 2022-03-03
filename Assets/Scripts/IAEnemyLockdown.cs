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
                if (presenteNaListaDeDeteccao)
                {
                    enemy.GeneralManager.EnemyManager.PerdiVisaoInimigo();
                    presenteNaListaDeDeteccao = false;
                    posicaoListaIndiceDeteccao = 0;
                }
                if (emLockDown)
                {
                    if(vendoPlayer)
                    {
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.PlayerDetectado;
                        verifiqueiUltimaPosicaoJogador = false;

                        AtivarIconeDeAlerta();
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.FazerRotinaLockdow;
                    }
                }
                else //voltar pro spawn
                {
                    if (inimigoEstados != InimigoEstados.IndoAtivarLockDown)
                    {
                        if (!vendoPlayer)
                        {
                            inimigoEstados = InimigoEstados.Patrulhar;
                        }
                        else if (vendoPlayer)
                        {
                            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.PlayerDetectado;
                            verifiqueiUltimaPosicaoJogador = false;

                            AtivarIconeDeAlerta();
                        }
                        else if (tomeiDano)
                        {
                            inimigoEstados = InimigoEstados.TomeiDano;
                            verifiqueiUltimaPosicaoJogador = false;
                        }
                    }
                }
                break;
            case EstadoDeteccaoPlayer.PlayerDetectado:
                vendoMorto = false;
                tomeiDano = false;
                vendoPlayer = vendoPlayerCircular;
                somTiro = false;
                somPasso = false;

                if (!presenteNaListaDeDeteccao) //sistema para ultimo integrante ter que apertar o botao, sempre ┤e a ultimo inimigo a ver o player quem vai ativar
                {
                    posicaoListaIndiceDeteccao = enemy.GeneralManager.EnemyManager.AdicionarAlguemVendoPlayer();
                    presenteNaListaDeDeteccao = true;
                }

                if (vendoPlayer)
                {
                    posicaoUltimoLugarVisto = posicaoAtualPlayer;

                    if (enemy.GeneralManager.EnemyManager.VerificarUltimoVerPlayer(posicaoListaIndiceDeteccao) && !emLockDown) //o ultimo inimigo a ver o player deve ativar o lockdown isso tem prioridade sobre atacar ou mover
                    {
                        inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                    }
                    else if (playerAreaAtaque)
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
                    if (inimigoEstados != InimigoEstados.IndoAtivarLockDown)
                    {
                        if(!verifiqueiUltimaPosicaoJogador)
                            {
                                inimigoEstados = InimigoEstados.AndandoUltimaPosicaoPlayerConhecida;
                            }
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
