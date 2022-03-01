using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorDeEntidadesProximasDaPorta : MonoBehaviour
{
    private Porta porta;

    private int entidadesColidindo;

    void Start()
    {
        porta = GetComponentInParent<Porta>();
        entidadesColidindo = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            entidadesColidindo++;

            if(collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            if(entidadesColidindo > 0)
            {
                if(porta.Trancado == false)
                {
                    switch (porta.GetTipoPorta)
                    {
                        case Porta.TipoPorta.Simples:
                            porta.AbrirPorta();
                            break;

                        case Porta.TipoPorta.Normal:
                            switch (porta.GetEstado)
                            {
                                case Porta.Estado.NaoLockdown:
                                    porta.AbrirPorta();
                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            entidadesColidindo--;

            if(entidadesColidindo <= 0)
            {
                if (porta.GetTipoPorta != Porta.TipoPorta.Contencao)
                {
                    porta.FecharPorta();
                }
            }
        }
    }
}
