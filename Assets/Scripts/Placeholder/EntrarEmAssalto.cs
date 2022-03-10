using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EntrarEmAssalto : MonoBehaviour
{
    [SerializeField] private string nomeScena;
    BoxCollider2D boxCollider;
    Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if(Colisao.HitTest(boxCollider,player.GetComponent<BoxCollider2D>()))
        {
            boxCollider.enabled = false;
            LevelLoaderScript.Instance.CarregarNivel(nomeScena);
        }
    }
}
