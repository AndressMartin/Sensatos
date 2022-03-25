using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : SingletonInstance<SaveManager>
{

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
    private void Start()
    {
        SaveSystem.Init();
    }

    /// <summary>
    /// Loads the game using the data on SaveData and the methods on SaveSystem. Afterwards, raises the OnLoadedGame event.
    /// </summary>
    public void OnLoadGame()
    {
        //Get the respective files from SaveSystem.Load()
        //Set SaveData.current
        var loadedSave = SaveSystem.Load();
        Debug.Log("Loaded game");
        SaveData.current.playerProfile = JsonUtility.FromJson<PlayerProfile>(loadedSave);
        //Invoke event for others to load
        onGameLoaded.Invoke();
    }
    /// <summary>
    /// First raises the event so that every script has a chance to save their content. 
    /// Then, Saves the game using the data on SaveData and the methods on SaveSystem.
    /// </summary>
    public void OnSaveGame()
    {
        onSavingGame.Invoke();
        SaveSystem.Save(SaveData.current.playerProfile);
        Debug.Log("Saved game");
        //Get the respetive files from SaveData and set on SaveSystem.Save();
    }

    public void DeleteSaves()
    {
        Debug.LogWarning("TODO: Implement delete saves");
    }
}
