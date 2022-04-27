using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderScript : MonoBehaviour
{
    public static LevelLoaderScript Instance { get; private set; }

    //Componentes
    [SerializeField] RectTransform loadingScreen;
    [SerializeField] Image barraDeCarregamento;
    [SerializeField] TMP_Text textoCarregando;

    //Variaveis
    [SerializeField] private string textoCarregandoPortugues;
    [SerializeField] private string textoCarregandoIngles;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    public void CarregarNivel(string ScenaIndex)
    {
        TrocarIdioma();

        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(ScenaIndex));
    }

    private IEnumerator LoadLevel(string levelIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01 (asyncLoad.progress);
            barraDeCarregamento.fillAmount = progress;
            yield return null;
        }

        if(asyncLoad.isDone)
        {
            loadingScreen.gameObject.SetActive(false);
        }
    }

    public void TrocarIdioma()
    {
        switch (IdiomaManager.GetIdiomaEnum)
        {
            case IdiomaManager.Idioma.Portugues:
                textoCarregando.text = textoCarregandoPortugues;
                break;

            case IdiomaManager.Idioma.Ingles:
                textoCarregando.text = textoCarregandoIngles;
                break;
        }
    }
}
