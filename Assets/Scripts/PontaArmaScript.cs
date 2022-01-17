using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontaArmaScript : MonoBehaviour
{
    public void AtualizarPontaArma(float offSetX, float offSetY) //Atualiza a ponta da arma
    {
        transform.position = new Vector2(transform.parent.transform.position.x + offSetX, transform.parent.transform.position.y + offSetY);
    }
}
