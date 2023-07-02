using UnityEngine;

namespace Entities
{
    /// <summary>
    /// This class represents the "body" / (virtual)physical conditions of an entity,
    /// such as its health, stamina, strength etc.
    /// Note that the values stored by the variables/attributes with _ prefixed only store the base values, it does not include effects from equipment, buffs, etc.
    /// To get the actual values, use the properties or Get methods. 
    /// </summary>
    public class EntityBody : MonoBehaviour
    {
        public int _Health; // Health of entity
        public int _Stamina; // Used for dashing
        public int _Mana; // Used for casting spells
        public int _Strength; // Increases physical damage
        public int _Speed; // Movement speed
        public int _Defense; // Reduces damage taken
        
        public int _CurrentHealth; // Automatically set to _Health on start
        public int _CurrentStamina; // Automatically set to _Stamina on start
        public int _CurrentMana; // Automatically set to _Mana on start

        public IStatusEffect[] StatusEffects; // Curent status effects on entity
        
        void Start()
        {
            _CurrentHealth = _Health;
            _CurrentStamina = _Stamina;
            _CurrentMana = _Mana;
        }
        public void Damage(int amt)
        {
            _Health -= amt;
        }
        // todo include methods to get the actual values that includes effects from equipment, buffs, etc.
    }
}