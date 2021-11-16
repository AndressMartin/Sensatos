using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

    // Start is called before the first frame update
    CircleCollider2D circleCollider2D;
    int circleRadiusDefault = 2;
    BoxCollider2D[] enemy;
    float raiotemp;
    float raioOriginal;
    [SerializeField] bool shoot = false;
    [SerializeField] bool agachado = false;
     private float time, timeMax;
    [SerializeField] private float timeMaxOriginal;
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        enemy = FindObjectsOfType<BoxCollider2D>();
        timeMax= timeMaxOriginal;
        raioOriginal = circleCollider2D.radius;

    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            circleCollider2D.radius = 5;
            shootCont();
        }

        else if (agachado)
        {
             circleCollider2D.radius = 0.1f;

        }
        else 
        {
            circleCollider2D.radius = 0.5f;

        }

    }
    void shootCont()
    {
        time += Time.deltaTime;
        foreach (BoxCollider2D item in enemy)
        {
            if (item.GetComponent<EnemyMove>() != null)
            {
                if (circleCollider2D.IsTouching(item))
                {
                    OnTriggerStay2D(item);
                }
            }

        }
        if (time > timeMax)
        {
            time = 0;
            timeMax = timeMaxOriginal;
            shoot = false;
        }
    }

    public void changeColliderRadius(float _raio)
    {
        raiotemp = _raio;
        if(_raio ==5)
        {
            shoot = true;
        }
        if(_raio ==1)
        {
            agachado = true;
        }
        if(_raio ==8)
        {
            agachado = false;
        }
        

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        EnemyMove enemyMoveTemp= collision.GetComponent<EnemyMove>();
        if(enemyMoveTemp != null)
        {
            enemyMoveTemp.HearEnemy(gameObject.GetComponentInParent<Player>(),circleCollider2D.radius);
        }
    }


}
