using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private List<GameObject> chaoTiles;
    [SerializeField] private List<GameObject> paredeLista;
     private GridTest gridTest;

    [SerializeField] private GameObject gameObjectTarget;
    bool gridChecked;
    // Start is called before the first frame update
    void Start()
    {
        gridTest = FindObjectOfType<GridTest>();
        chaoTiles = gridTest.chaolista;
        paredeLista = gridTest.walllista;
    }

    // Update is called once per frame
    void Update()
    {
        if(chaoTiles.Count == 0)
            chaoTiles = gridTest.chaolista;

        if (!gridChecked)
            CheckGrid();


        gridTest.paintPlayerTile(gameObjectTarget);
    }
    public void Movement()
    {
        //gameObjectTarget = _gameObject;
    }
    public void ReceivePlayerGameObject(GameObject _gameObject)
    {
        gameObjectTarget = _gameObject;
    }

    void CheckGrid()
    {
        foreach (GameObject item in chaoTiles)
        {
            if (item.GetComponent<GridCell>() != null)
            {
                
                //Debug.Log(item.GetComponent<GridCell>().tipo);
            }
        }
        gridChecked = true;
    }
}
