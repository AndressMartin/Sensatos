using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public GameObject tileGameObject;
    public GameObject objetoAcima;
    public string tipo;
    private bool cellChecked;
    [SerializeField] private GridTest gridTest;
    // Start is called before the first frame update
    void Start()
    {
        tileGameObject = gameObject;
        gridTest = FindObjectOfType<GridTest>();
        tipo = "Chao";
        gridTest.AddToTotalList(gameObject);
        transform.parent = gridTest.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitboxTile")
        {
            if(collision.GetComponentInParent<Player>() != null)
            checkPlayer(collision);

            else if(collision.GetComponentInParent<PathFinding>())
            checkEnemy();
        }
        if (collision.GetComponent<Cerca>() != null)
            checkWall();
    }

 
    void checkEnemy()
    {
        tipo = "Enemy";
        gridTest.AddEnemyList(gameObject);
        cellChecked = true;
    }
    void checkPlayer(Collider2D collider)
    {         
        tipo = "Player";
        cellChecked = true;
        objetoAcima = collider.GetComponentInParent<BoxCollider2D>().gameObject;
        gridTest.AddPlayerlList(gameObject, this);

    }
    void checkWall()
    {
        tipo = "Parede";
        gridTest.AddToWallList(gameObject);         
        cellChecked = true;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bool player = false;
        bool enemy = false;
        if (collision.gameObject.tag == "HitboxTile")
        {

            if (collision.GetComponentInParent<PathFinding>() != null)
            {
                enemy = true;
            }
            else
            {
                player = true;
            }

            if (enemy)
            {
                gridTest.RemoveEnemyList(gameObject);
                tipo = "Chao";

            }

            else if (player)
            {

                tipo = "Chao";
                gridTest.RemovePlayerList(this);
            }
            cellChecked = false;
        }

    }
}
