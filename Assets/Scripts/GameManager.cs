using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public PlayerController player;
    public GameObject inventoryUi;

    private void Awake()
    {
        if (instance != null) Debug.LogError("Warning: multiple instances of GameManager found! The static instance will be changed to this one!!!! This is probably not what you want!");
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Inventory Toggle")) inventoryUi.SetActive(!inventoryUi.activeSelf);
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
}