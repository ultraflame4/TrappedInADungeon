using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "WeaponFollowConfig", menuName = "GameConfig/WeaponFollowConfig", order = 0)]
    public class WeaponFollowConfig : ScriptableObject
    {
        public float followOffset = 0.4f;
        public float indexOffset = 0.3f;
        public float travelSpeed = 0.2f;
        public float attackTravelSpeed = 0.8f;
    }
}