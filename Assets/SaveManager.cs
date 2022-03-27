using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    //Instancia do singleton
    public static SaveManager instance = null;

    private UnityEvent onGameLoaded,
                       onSavingGame;

    public UnityEvent OnGameLoaded 
    {
        get
        {
            if (onGameLoaded == null)
                onGameLoaded = new UnityEvent();
            return onGameLoaded;
        }
        set
        {
            if (onGameLoaded == null)
                onGameLoaded = new UnityEvent();
            onGameLoaded = value;
        }
    }

    public UnityEvent OnSavingGame
    {
        get
        {
            if (onSavingGame == null)
                onSavingGame = new UnityEvent();
            return onSavingGame;
        }
        set
        {
            if (onSavingGame == null)
                onSavingGame = new UnityEvent();
            onSavingGame = value;
        }
    }

    private void Awake()
    {
        //Faz do script um singleton
        if (instance == null) //Confere se a instancia nao e nula
        {
            instance = this;
            Debug.Log("Setei a instancia do save manager");
        }
        else if (instance != this) //Caso a instancia nao seja nula e nao seja este objeto, ele se destroi
        {
            Destroy(gameObject);
            return;
        }

        //Cria a pasta de saves, se ela nao existir
        Save.IniciarPasta();
    }

    public void SalvarJogo(int slot)
    {
        onSavingGame?.Invoke();

        Save.Salvar(slot);
    }

    public void CarregarJogo(int slot)
    {
        Save.Carregar(slot);

        onGameLoaded?.Invoke();
    }
}
