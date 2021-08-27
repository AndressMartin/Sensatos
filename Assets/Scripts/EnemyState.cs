using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int vida=50;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TomarDano(int dano)
    {       
        vida -= dano;
        StartCoroutine(Flash());

        if (vida <= 0)
            Destroy();
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;

    }

    void Destroy()
    {
        //Destroy(gameObject);
    }
}
