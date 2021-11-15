using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArma : EntityModel
{
    [SerializeField]private float distanceFromChar;
    public void AtualizarPontaArma(Direcao _direcao)    // Atualiza a ponta da arma
    {
        direcao = _direcao;
    }
    
}
