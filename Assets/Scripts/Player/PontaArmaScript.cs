using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArmaScript : MonoBehaviour
{
    public void AtualizarPontaArma(Vector2 offSet) //Atualiza a ponta da arma
    {
        transform.position = (Vector2)transform.parent.transform.position + offSet;
    }
}
