using System;
using Core.Save;
using EasyButtons;
using Level;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour, ISaveHandler
{
    public GameObject inventoryUi;
    public LevelGenerator levelGenerator;
    public bool SpawnEnemies = true;
    public GameControls inputs;

    public static GameManager Instance { get; private set; }
    public static GameControls Controls => Instance.inputs;

    public static int CurrentAreaIndex { get; private set; } = 0;

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
        GameSaveManager.AddSaveHandler("game", this);
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
        EditorUtility.RevealInFinder(GameSaveManager.GetSavePath());
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    //todo add ui support for gamepad mouse 

    private void Start()
    {
        inventoryUi.SetActive(false);
        Controls.Menus.InventoryToggle.performed += (ctx) => inventoryUi.SetActive(!inventoryUi.activeSelf);
        levelGenerator.AreaIndex = CurrentAreaIndex;
        levelGenerator.GenerateLevel();
        GameSaveManager.LoadSave();
    }

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

    [Serializable]
    class SaveData
    {
        public int currentAreaIndex;
    }

    private SaveData currentSaveData => new SaveData {
            currentAreaIndex = CurrentAreaIndex
    };
    
    public void OnLoadSave(string json)
    {
        var saveData = currentSaveData;
        JsonUtility.FromJsonOverwrite(json,saveData);
        CurrentAreaIndex = saveData.currentAreaIndex;
    }

    public string OnWriteSave()
    {
        return JsonUtility.ToJson(currentSaveData,true);
    }
    
}