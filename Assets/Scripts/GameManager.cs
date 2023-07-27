using System;
using Core.Save;
using Core.UI;
using Core.Utils;
using EasyButtons;
using Level;
using Newtonsoft.Json;
using UI.SceneTransition;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[JsonObject(MemberSerialization.OptIn)]
public class GameManager : MonoBehaviour, ISaveHandler
{
    public bool SpawnEnemies = true;
    public bool LoadGameSave = true;
    private GameControls inputs;
    public SceneTransitionController sceneTrans;

    public static GameManager Instance { get; private set; }
    public static GameControls Controls => Instance.inputs;

    [JsonProperty]
    public static int CurrentAreaIndex { get; private set; } = 0;

    public VolatileValue<bool> GamePaused = new();
    public event Action GenerateLevelEvent;
    [Tooltip("Whether this GameManager is in a level scene or not. If true, level specific events such as GamePaused.Changed will be disabled.")]
    public bool isLevelScene = true;
    public static string CurrentSaveName { get; private set; } = "DefaultSave";
    void Awake()
    {
        Time.timeScale = 1; // Make sure time scale is set to 1 when starting
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
        // No need to register pause event handlers if not in level
        if (isLevelScene)
        {
            inputs.Menus.Pause.performed += (ctx) => { GamePaused.value = !GamePaused.value; };
            GamePaused.Changed += UpdateTimeScale;
            GameSaveManager.AddSaveHandler("game", this);
        }
        
    }
    
    private void Start()
    {
        if (LoadGameSave)
        {
            GameSaveManager.LoadSave(CurrentSaveName);
        }
        if (isLevelScene)
        {
            GenerateLevelEvent?.Invoke();
        }
        sceneTrans?.FadeOut();
    }
    

    void UpdateTimeScale()
    {
        Time.timeScale = (GamePaused.value) ? 0 : 1;
    }
    
    public void LoadGame(string saveName = "DefaultSave")
    {
        CurrentSaveName = saveName;
        sceneTrans?.TransitionToScene("GameLevel");
    }

    /// <summary>
    /// Clears the current save
    /// </summary>
    [Button]
    public void ClearSave()
    {
        GameSaveManager.DeleteSave(CurrentSaveName);
    }

    /// <summary>
    /// Loads the current save
    /// </summary>
    /// <param name="saveName"></param>
    [Button]
    public void LoadSave()
    {
        GameSaveManager.LoadSave(CurrentSaveName);
    }

    /// <summary>
    /// Writes to the current save
    /// </summary>
    [Button]
    public void WriteSave()
    {
        if (!isLevelScene) return; // No need to write to save if not in level as WriteSave is intended for level progress / saves
        GameSaveManager.WriteSave(CurrentSaveName);
    }

    [Button]
    public void OpenSaveLocation()
    {
#if UNITY_EDITOR
        EditorUtility.RevealInFinder(GameSaveManager.GetSavePath());
#endif
    }

    //todo add ui support for gamepad mouse 


    public void LoadNextArea()
    {
        CurrentAreaIndex++;
        sceneTrans?.TransitionToScene("GameLevel");
    }

    public void LoadPrevArea()
    {
        CurrentAreaIndex = Mathf.Max(0, CurrentAreaIndex - 1);
        sceneTrans?.TransitionToScene("GameLevel");
    }

    public void QuitToMainMenu()
    {
        WriteSave();
        sceneTrans?.TransitionToScene("MainMenu");
    }

    public void QuitGame()
    {
        WriteSave();
        Application.Quit();
    }

    private void OnDestroy()
    {
        WriteSave();
        GameSaveManager.ClearSaveHandlers();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}