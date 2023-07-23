using Core.Save;
using EasyButtons;
using Level;
using Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
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
}