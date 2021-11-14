using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GridTest : MonoBehaviour
{
    [SerializeField] private int row, cols;
    [SerializeField] private float cellSize;
    [SerializeField] private GameObject square;
    [SerializeField] private bool groundChecked;

    public List<GameObject> enemyLista;
    public List<GameObject> walllista;
    public List<GameObject> chaolista;
    public List<GameObject> totalLista;
    public List<GameObject> tempLista;

    public List<GameObject> playerLista;
    public List<GridCell> playerListaGameObjects;
    bool created;
    void Start()
    {
        CreateGird();
    }
    void CreateGird()
    {
        int cont = 0;
        for(int i=0;i < row; i++)
        {
            for(int j=0;j < cols; j++)
            {
                GameObject grid = Instantiate(square,new Vector3(transform.position.x+i+cellSize, transform.position.y+j + cellSize,0),new Quaternion(0,0,0,0));
                grid.transform.name = "Grid "+cont;
                cont++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        VerifiyGround();
        PaintTile();
    }
    public void paintPlayerTile(GameObject _gameObject)
    {
        if (_gameObject != null)
        {
            foreach (GridCell grid in playerListaGameObjects)
            {
                grid.tileGameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            foreach (GameObject game in enemyLista)
            {
                game.GetComponent<SpriteRenderer>().color = Color.red;
            }
            
        }
        else 
        {
            /*foreach (GridCell grid in playerListaGameObjects)
            {
                grid.tileGameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            foreach (GameObject game in enemyLista)
            {
                game.GetComponent<SpriteRenderer>().color = Color.white;
            }*/
        }

     }
    void VerifiyGround()
    {
        if (!groundChecked)
        {
            if (totalLista.Count >= row * cols)
                foreach (GameObject wall in walllista)
                {
                    foreach (GameObject total in totalLista)
                    {
                        if (total.GetComponent<GridCell>().tipo == "Chao")
                        {
                            tempLista.Add(total);
                        }
                        else
                        {
                        }
                    }
                }    
            if (tempLista.Count >= totalLista.Count - walllista.Count)
            {
                chaolista = tempLista.Distinct().ToList();
                groundChecked = true;
                tempLista = null;
            }
           
        }

    }
    void PaintTile()
    {
        /*foreach (GridCell grid in playerListaGameObjects)
        {
            grid.tileGameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        foreach (GameObject game in enemyLista)
        {
            game.GetComponent<SpriteRenderer>().color = Color.red;
        }*/
    }

    public void AddToWallList(GameObject _gameObject)
    {
        if (!walllista.Contains(_gameObject))
            walllista.Add(_gameObject);
    }
    public void AddToChaollList(GameObject _gameObject)
    {
        if (!chaolista.Contains(_gameObject))
            chaolista.Add(_gameObject);
    }

    public void AddToTotalList(GameObject _gameObject)
    {
        if (!totalLista.Contains(_gameObject))
            totalLista.Add(_gameObject);
    }

    public void AddPlayerlList(GameObject _gameObjectTile, GridCell _gridCell)
    {
        if (!playerLista.Contains(_gameObjectTile))
        {
            playerLista.Add(_gameObjectTile);
            //playerListaGameObjects.Add(_gridCell);
        }

        if (!playerListaGameObjects.Contains(_gridCell))
        {
            playerListaGameObjects.Add(_gridCell);
        }
        
    }
 

    public void AddEnemyList(GameObject _gameObject)
    {
        if(!enemyLista.Contains(_gameObject))
            enemyLista.Add(_gameObject);
    }

    public void RemovePlayerList(GridCell _gridCell)
    {
        foreach (GameObject game in playerLista)
        {
            game.GetComponent<SpriteRenderer>().color = Color.white;
        }
        playerLista.Clear();

        playerListaGameObjects.Remove(_gridCell);

    }
    public void RemoveEnemyList(GameObject _gameObject)
    {
        foreach (GameObject game in enemyLista)
        {
            game.GetComponent<SpriteRenderer>().color = Color.white;
        }
        //enemyLista.Clear();

        enemyLista.Remove(_gameObject);

    }
}

