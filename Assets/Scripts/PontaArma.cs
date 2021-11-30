using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArma : EntityModel
{
    public void AtualizarPontaArma(Direcao _direcao) //Atualiza a ponta da arma
    {
        direcao = _direcao;
        transform.position = FrenteDoPersonagem(transform.parent.transform, 0.5f, 1);
    }
    
}
