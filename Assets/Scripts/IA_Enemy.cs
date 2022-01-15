using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Enemy : MonoBehaviour
{
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
   /*     if (enemy.vendoPlayer)//caso esteja vendo o player
        {
            switch (estado)
            {
                case Estado.Combate:
                    seguirAtacarPlayer();
                    break;
                case Estado.Alerta:
                    counterAlert();//inicia o contador pra ir entrar em estado combate 
                    break;
                case Estado.Rotina:
                    estado = Estado.Alerta;
                    break;
                case Estado.Lockdown:
                    seguirAtacarPlayer();
                    break;

            }

        }
        else //caso não veja o player
        {
            switch (estado)
            {
                case Estado.Combate:
                    estado = Estado.Alerta;
                    break;

                case Estado.Lockdown:
                    switch (stance)
                    {
                        case Stances.CorrerAteUltimaPosicaoPlayer:
                            MoveLockDown(lastPlayerPosition);
                            break;
                        case Stances.VarrerFase:
                            VarrerFase();
                            break;

                        default:
                            Debug.Log("ERROR");
                            break;
                    }
                    break;

                case Estado.Alerta://ver player
                    switch (fazerMovimentoAlerta)
                    {
                        case FazerMovimentoAlerta.AndandoAte_UltimaPosicaoPlayer:
                            MovimentarUltimaPosicaoPlayer(lastPlayerPosition);
                            break;
                        case FazerMovimentoAlerta.ChechandoUltimaPosicaoPlayer:
                            VerificarRegiao();
                            break;
                        case FazerMovimentoAlerta.VoltandoA_RotinaPadrao:
                            MovimentarVoltarRotinaPadrao(ultimaposicaoOrigem);
                            break;
                        case FazerMovimentoAlerta.OuviuTiro:
                            OuviuTiro();
                            break;
                        case FazerMovimentoAlerta.AndandoAte_UltimaPosicaoSomPlayer:
                            MovimentarAteOSom();
                            break;
                        case FazerMovimentoAlerta.OuviuPassos:
                            OlharDirecaoSom();
                            break;

                        case FazerMovimentoAlerta.NA:
                            estado = Estado.Rotina;
                            break;
                    }
                    break;


                case Estado.Rotina: //fazendo rotina
                    if (hearPlayer || hearShoot)//hearPlayer
                    {
                        OuvindoInimigo();
                    }
                    else
                    {
                        switch (stance)
                        {
                            case Stances.Patrolling://patrulhando
                                Patrulhar();
                                break;
                        }
                    }
                    break;

            }
        }*/
    }
}
