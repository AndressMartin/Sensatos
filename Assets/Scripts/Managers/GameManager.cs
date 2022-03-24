using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        //Carrega as configuracoes do arquivo quando o jogo se inicia
        SaveConfiguracoes.CarregarConfiguracoes();

        //Atualiza o idioma
        IdiomaManager idiomaManager = gameObject.AddComponent<IdiomaManager>() as IdiomaManager;
        idiomaManager.SetIdioma(SaveConfiguracoes.configuracoes.idioma);
        Destroy(idiomaManager);
    }
}
