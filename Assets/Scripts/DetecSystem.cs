using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class DetecSystem : MonoBehaviour
{
     private TMP_Text textoDeteccao;
     private TMP_Text textoFugindo;
     private EnemyMovement item;

    [SerializeField] float valorDetect;
    [SerializeField] float valorFugindo;

    [SerializeField] float tempoPraEntarEmAlertaMax;
    [SerializeField] float tempoPraResetarAlertaMax;
    // Start is called before the first frame update
    void Start()
    {
        TMP_Text[] temp;
        temp = GetComponentsInChildren<TMP_Text>();

        textoDeteccao = temp[0];
        textoFugindo = temp[1];

        item = transform.parent.GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    public void Main()
    {
        if (item.RetornarMorto())//não funciona porque o fato de estar morto acaba não chamando o main,teria que colocar quando morrer passa infos pra ca mas mt role
        {
            textoDeteccao.text = "Detecão % Morto";
            textoFugindo.text = "Fugindo % Morto";

        }
        else
        {

            bool emAlerta = false;

            if (item.RetornarVendoPlayer())
            {
                emAlerta = true;
            }

            if (emAlerta)
            {
                float valorMax = 0.0f;
                if (item.RetornarTimerDeAlerta() > valorMax)
                    valorMax = item.RetornarTimerDeAlerta();
                else
                    valorMax = item.RetornarTimerDeAlerta();

                valorDetect = (valorMax / tempoPraEntarEmAlertaMax) * 100;
                textoDeteccao.text = "Detecão % " + valorDetect.ToString();
            }

            else
            {
                bool zerou = false;
                float valorMax = 0.0f;
                if (item.RetornarTimerDeAlerta() > valorMax)
                    valorMax = item.RetornarTimeResetandoAlerta();

                else
                    valorMax = item.RetornarTimeResetandoAlerta();

                valorFugindo = (valorMax / tempoPraResetarAlertaMax) * 100;
                textoFugindo.text = "Fugindo % " + valorFugindo.ToString();

                if (valorFugindo >= 99)
                {
                    zerou = true;
                }
                if (zerou)
                {
                    textoDeteccao.text = "Detecão % " + 0;
                }
            }
        }
    }
    public void AddtoLista(float tempoMax, float tempoResetMax)
    {
        tempoPraEntarEmAlertaMax = tempoMax;
        tempoPraResetarAlertaMax = tempoResetMax;
    }
}

