using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using Item;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryUi;
    private static GameManager instance; 
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Warning: multiple instances of GameManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
        }
        instance = this;
    }
    public static GameManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        inventoryUi.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory Toggle"))
        {
            inventoryUi.SetActive(!inventoryUi.activeSelf);
        }
    }
}
