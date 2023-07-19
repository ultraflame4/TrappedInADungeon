using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryUi;
    public bool SpawnEnemies = true;
    public GameControls inputs;

    public static GameManager Instance { get; private set; }
    public static GameControls Controls => Instance.inputs;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Warning: multiple instances of GameManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (inputs == null)
        {
            inputs = new GameControls();
        }

        inputs.Enable();
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
    }
}