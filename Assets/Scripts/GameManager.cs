using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using Item;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public WeaponItem[] Weapons;
    public SkillItem[] Skills;

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Gives a wood sword to the player.
    /// Mainly for debugging purposes
    /// </summary>
    [Button]
    void GiveWoodSword()
    {
        player.Inventory.AddItem(new WeaponItemInstance(Weapons[0]));
    }
    /// <summary>
    /// Clears the player inventory
    /// Mainly for debugging purposes
    /// </summary>
    [Button]
    void ClearPlayerInventory()
    {
        player.Inventory.Clear();
    }
}
