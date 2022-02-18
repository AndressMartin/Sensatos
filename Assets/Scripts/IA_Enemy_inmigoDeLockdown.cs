using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class IA_Enemy_inmigoDeLockdown : IA_Enemy_Basico
{
    [SerializeField] Vector2 SalaSegurança;
    [SerializeField] Vector2 PosicaoDeSpawn;

    //[SerializeField]new bool emLockDown;
    public override void Start()
    {
        base.Start();
        transform.position = SalaSegurança;
        PosicaoDeSpawn = transform.position;
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
                        estadoDeteccaoPlayer = EstadoDeteccaoPlayer.playerDetectado;
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
            case EstadoDeteccaoPlayer.playerDetectado:
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
    public override void Respawn()
    {
        base.Respawn();
        transform.position = PosicaoDeSpawn;
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
    public override void SerSpawnado(Vector2 _pontoSpawn,AILerp lerp)
    {
        base.SerSpawnado(_pontoSpawn,lerp);
        SalaSegurança = _pontoSpawn;
    }
    protected override void Patrulhar()
    {
        if (VerificarChegouAteAlvo(SalaSegurança))
        {
            gameObject.SetActive(false);
        }
    }

}
