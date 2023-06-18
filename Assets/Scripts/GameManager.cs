using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using Item;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory Toggle"))
        {
            inventoryUi.SetActive(!inventoryUi.activeSelf);
        }
    }

}
