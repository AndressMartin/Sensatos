using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderScript : MonoBehaviour
{
    public static LevelLoaderScript Instance { get; private set; }

    [SerializeField] RectTransform LoadingScreen;
    [SerializeField] Slider slider;
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
        LoadingScreen = GetComponentInChildren<RectTransform>();
        slider = LoadingScreen.GetComponentInChildren<Slider>();
    }
    public void CarregarNivel(string ScenaIndex)
    {
        StartCoroutine(LoadLevel(ScenaIndex));
    }
    IEnumerator LoadLevel(string levelIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        LoadingScreen.gameObject.SetActive(true);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01 (asyncLoad.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
        if(asyncLoad.isDone)
        {
            LoadingScreen.gameObject.SetActive(false);
        }

    }

}
