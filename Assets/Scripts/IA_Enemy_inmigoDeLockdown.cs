using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    }
                    else
                    {
                        inimigoEstados = InimigoEstados.Patrulhar;
                    }
                }
                else//voltar pro spawn
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
    public override void SerSpawnado(Vector2 _pontoSpawn)
    {
        SalaSegurança = _pontoSpawn;
    }
    protected override void Patrulhar()
    {
        base.Patrulhar();
    }
    protected override void FazerRotinaLockdown()
    {
        if(VerificarChegouAteAlvo(SalaSegurança))
        {
            gameObject.SetActive(false);
        }
        //Debug.Log("indo pra origem");
       // base.FazerRotinaLockdown();
    }
   
}
