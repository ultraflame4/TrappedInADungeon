using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryUi;
    private static GameManager instance;
    public GameControls inputs;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Warning: multiple instances of GameManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
        }

        instance = this;
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
    public static GameManager Instance => instance;
    
    public static GameControls Controls => instance.inputs;

    private void Start()
    {
        inventoryUi.SetActive(false);
        Controls.Menus.InventoryToggle.performed += (ctx) => inventoryUi.SetActive(!inventoryUi.activeSelf);
    }
    
}