using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteEfeito : MonoBehaviour
{
    private EfeitosVisuais efeitosVisuais;
    public EfeitosVisuais efeito2;

    private void Start()
    {
        efeitosVisuais = GetComponent<EfeitosVisuais>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            efeitosVisuais.SetTintEffect(new Color(0, 1, 0, 1), 6f);
            //efeito2.SetTintEffect(new Color(1, 0, 1, 1), 3f);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            efeitosVisuais.SetTintSolidEffect(new Color(1, 0, 0, 1), 3f);
            efeito2.SetTintSolidEffect(new Color(1, 1, 0, 1), 1f);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            //efeitosVisuais.SetTintEffect(new Color(0, 0, 1, 1), 5f);
            efeito2.SetTintEffect(new Color(0, 1, 1, 1), 2f);
        }
    }
}
