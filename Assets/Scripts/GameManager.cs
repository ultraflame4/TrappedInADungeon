using System;
using Core.Save;
using Core.UI;
using Core.Utils;
using EasyButtons;
using Level;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[JsonObject(MemberSerialization.OptIn)]
public class GameManager : MonoBehaviour, ISaveHandler
{
    public LevelGenerator levelGenerator;
    public bool SpawnEnemies = true;
    public bool LoadGameSave = true;
    public GameControls inputs;
    
    public static GameManager Instance { get; private set; }
    public static GameControls Controls => Instance.inputs;

    [JsonProperty]
    public static int CurrentAreaIndex { get; private set; } = 0;
    public VolatileValue<bool> GamePaused  = new();
    public event Action GenerateLevelEvent;
    void Awake()
    {
        if (inputs == null)
        {
            inputs = new GameControls();
        }

        inputs.Enable();
        if (Instance != null)
        {
            Debug.LogError("Warning: multiple instances of GameManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
        }

        Instance = this;
        inputs.Menus.Pause.performed += (ctx) =>
        {
            GamePaused.value = !GamePaused.value;
        };
        GamePaused.Changed += OnGamePausedChanged;
        GameSaveManager.AddSaveHandler("game", this);
    }
    
    private void Start()
    {
        
        if (LoadGameSave)
        {
            GameSaveManager.LoadSave();
        }
     


    }


    [Button]
    public void ClearSave()
    {
        GameSaveManager.DeleteSave();
    }

    [Button]
    public void LoadSave()
    {
        GameSaveManager.LoadSave();
    }

    [Button]
    public void WriteSave()
    {
        GameSaveManager.WriteSave();
    }
    [Button]
    public void OpenSaveLocation()
    {
        #if UNITY_EDITOR
        EditorUtility.RevealInFinder(GameSaveManager.GetSavePath());
        #endif
    }

    void OnGamePausedChanged()
    {
        Time.timeScale = GamePaused.value ? 0 : 1;
    }

    //todo add ui support for gamepad mouse 


    public void LoadNextArea()
    {
        CurrentAreaIndex++;
        SceneManager.LoadScene("GameLevel");
    }

    public void LoadPrevArea()
    {
        CurrentAreaIndex = Mathf.Max(0, CurrentAreaIndex - 1);
        SceneManager.LoadScene("GameLevel");
    }
    
    public void QuitToMainMenu()
    {
        WriteSave();
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        WriteSave();
        Application.Quit();
    }
    private void OnDestroy()
    {
        GameSaveManager.WriteSave();
        GameSaveManager.ClearSaveHandlers();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }
}