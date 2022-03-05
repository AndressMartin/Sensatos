using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IAEnemyLockdown : IAEnemy
{
    [SerializeField] Vector2 salaSegurança;

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

        tipoInimigo = TipoInimigo.Lockdown;
        salaSegurança = transform.position;
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
                    viuPlayerAlgumaVez = false;

                    if (vendoPlayer)
                    {
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.PlayerDetectado;
                        verifiqueiUltimaPosicaoJogador = false;

                        AtivarIconeDeAlerta();
                    }
                    else if (tomeiDano && inimigoEstados != InimigoEstados.TomeiDano)
                    {
                        inimigoEstados = InimigoEstados.TomeiDano;
                        verifiqueiUltimaPosicaoJogador = false;
                        posicaoUltimoLugarVisto = posicaoAtualPlayer;
                    }
                    else if (!vendoPlayer && !tomeiDano)
                    {
                        inimigoEstados = InimigoEstados.FazerRotinaLockdown;
                    }
                }
                else //voltar pro spawn
                {
                    if (tomeiDano && inimigoEstados != InimigoEstados.TomeiDano)
                    {
                        inimigoEstados = InimigoEstados.TomeiDano;
                        verifiqueiUltimaPosicaoJogador = false;
                        posicaoUltimoLugarVisto = posicaoAtualPlayer;
                    }
                    else if (inimigoEstados != InimigoEstados.IndoAtivarLockDown)
                    {               
                        if (vendoPlayer)
                        {
                            estadoDeteccaoPlayer = EstadoDeteccaoPlayer.PlayerDetectado;
                            verifiqueiUltimaPosicaoJogador = false;

                            AtivarIconeDeAlerta();
                        }
                        else if(!vendoPlayer  && !tomeiDano && viuPlayerAlgumaVez)
                        {
                            inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                        }
                        else if (!vendoPlayer && !tomeiDano && viuPlayerAlgumaVez)
                        {
                            inimigoEstados = InimigoEstados.IndoAtivarLockDown;
                        }
                        else if (!vendoPlayer && !tomeiDano && !viuPlayerAlgumaVez)
                        {
                            inimigoEstados = InimigoEstados.Patrulhar;
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
                viuPlayerAlgumaVez = true;

                if (!presenteNaListaDeDeteccao) //sistema para ultimo integrante ter que apertar o botao, sempre ´e a ultimo inimigo a ver o player quem vai ativar
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
                    else if (movementRange)
                    {
                        //Debug.Log("Ele ta longe de min nao posso Atacar");
                        inimigoEstados = InimigoEstados.FicarParado;
                        if (playerAreaAtaque)
                        {
                            Atacar();
                        }
                    }

                    else if (tomeiDano)
                    {
                        inimigoEstados = InimigoEstados.AndandoAtePlayer;
                    }

                    else
                    {
                        //Debug.Log("Ele ta longe de min nao posso Atacar");
                        inimigoEstados = InimigoEstados.AndandoAtePlayer;
                        if (playerAreaAtaque)
                        {
                            Atacar();
                        }
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
    protected override void Patrulhar()
    {
        if (VerificarChegouAteAlvo(salaSegurança))
        {
            gameObject.SetActive(false);
        }
    }

}
