using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

    // Start is called before the first frame update
    CircleCollider2D circleCollider2D;
    int circleRadiusDefault = 2;
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(circleCollider2D.radius);
        if (circleCollider2D.radius != circleRadiusDefault)
        {
            circleCollider2D.radius = circleRadiusDefault;
        }*/
    }

    public void changeColliderRadius(int _raio)
    {
        Debug.Log(_raio);
        if (circleCollider2D.radius != _raio)
        {
            circleCollider2D.radius = _raio;
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
