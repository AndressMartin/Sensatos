using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class ModoLockDown : MonoBehaviour
{
    [SerializeField] List<EnemyMove> enemies = new List<EnemyMove>();
    [SerializeField] List<EnemyMove> enemiesMortos = new List<EnemyMove>();

    [SerializeField] private TMP_Text textoDetecção;
    [SerializeField] private TMP_Text textoFugindo;

    [SerializeField] public Image image;
    float tempoPraEntarEmAlertaMax;
    float tempoPraResetarAlertaMax;

    float valorDetect;
    float valorFugindo;
    // Start is called before the first frame update
    void Start()
    {
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
        foreach (EnemyMove item in enemies)
        {
            if (item.RetornarVendoPlayer()) 
            {
                emAlerta = true;
                
            }
            if(item.RetornarMorto())
            {
                addMorto(item);
            }
            else
            {
                if(enemiesMortos.Contains(item))
                {
                    enemiesMortos.Remove(item);
                }
            }
            Debug.Log(item.RetornarMorto());
        }
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
            textoDetecção.text = "Detecão % "+ valorDetect.ToString();
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

    public void AddtoLista(EnemyMove enemy,float tempoMax,float tempoResetMax)
    {
        if(!enemies.Contains(enemy))
        {
            enemies.Add(enemy);

            tempoPraEntarEmAlertaMax = tempoMax;
            tempoPraResetarAlertaMax = tempoResetMax;

            enemy.GetComponentInChildren<DetecSystem>().AddtoLista(tempoMax, tempoResetMax);
        }
        
    }
    void addMorto(EnemyMove enemy)
    {
        if (!enemiesMortos.Contains(enemy))
        {
            enemiesMortos.Add(enemy);
        }
    }
}
