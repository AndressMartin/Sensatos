using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class ModoLockDown : MonoBehaviour
{
    [SerializeField] List<EnemyMovement> enemies = new List<EnemyMovement>();
    [SerializeField] List<EnemyMovement> enemiesMortos = new List<EnemyMovement>();

    [SerializeField] private TMP_Text textoDetecção;
    [SerializeField] private TMP_Text textoFugindo;

    LockDownManager lockDownManager;
    float tempoPraEntarEmAlertaMax;
    float tempoPraResetarAlertaMax;

    float valorDetect;
    float valorFugindo;

    bool controle = false;
    // Start is called before the first frame update
    void Start()
    {
        lockDownManager = FindObjectOfType<LockDownManager>();
        //image=GetComponent<Sprite>();
        /*Component[] components = gameObject.GetComponents(typeof(Component));
        foreach (Component component in components)
        {
            Debug.Log(component);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        bool emAlerta = false;
        foreach (EnemyMovement item in enemies)
        {
            if (item.RetornarMorto())
            {
                addMorto(item);
            }
            else if (item.RetornarVendoPlayer())
            {
                emAlerta = true;
            }

            if (!item.RetornarMorto())
            {
                if (enemiesMortos.Contains(item))
                {
                    enemiesMortos.Remove(item);
                }
            }
        }
        ZerarHudQuandoNãoTiverInimigos();

        if (!controle)
        {
            if (emAlerta)
            {
                float valorMax = 0.0f;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (i != 0)
                    {
                        if (enemies[i].RetornarTimerDeAlerta() > valorMax)
                            valorMax = enemies[i].RetornarTimerDeAlerta();
                    }
                    else
                        valorMax = enemies[i].RetornarTimerDeAlerta();
                }
                valorDetect = (valorMax / tempoPraEntarEmAlertaMax) * 100;
                textoDetecção.text = "Detecão % " + valorDetect.ToString();
            }
            else
            {
                bool zerou = false;
                float valorMax = 0.0f;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (i != 0)
                    {
                        if (enemies[i].RetornarTimerDeAlerta() > valorMax)
                            valorMax = enemies[i].RetornarTimeResetandoAlerta();
                    }
                    else
                        valorMax = enemies[i].RetornarTimeResetandoAlerta();
                }


                valorFugindo = (valorMax / tempoPraResetarAlertaMax) * 100;

                textoFugindo.text = "Fugindo % " + valorFugindo.ToString();

                if (valorFugindo >= 99)
                {
                    zerou = true;
                }

                if (zerou)
                {
                    textoDetecção.text = "Detecão % " + 0;
                }
            }
        }
    }
    void ZerarHudQuandoNãoTiverInimigos()
    {
        if (enemies.Count == enemiesMortos.Count)
        {
            controle = true;
        }
        else
        {
            controle = false;
            textoDetecção.text = "Detecão % " + 0;
            textoFugindo.text = "Fugindo % " + 0;

        }
    }
    public void AddtoLista(EnemyMovement enemy, float tempoMax, float tempoResetMax)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);

            tempoPraEntarEmAlertaMax = tempoMax;
            tempoPraResetarAlertaMax = tempoResetMax;

            enemy.GetComponentInChildren<DetecSystem>().AddtoLista(tempoMax, tempoResetMax);
        }

    }
    void addMorto(EnemyMovement enemy)
    {
        if (!enemiesMortos.Contains(enemy))
        {
            enemiesMortos.Add(enemy);
        }
    }

}
