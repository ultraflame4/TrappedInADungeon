namespace Item
{
    
    public class ItemInstance
    {
        public ItemScriptableObject itemType;
        public ItemInstance(ItemScriptableObject itemType)
        {
            this.itemType = itemType;
        }
    }
    public class WeaponItemInstance : ItemInstance
    {
        public WeaponItemInstance(WeaponItem itemType) : base(itemType)
        {
            
        }
    }
    public class SkillItemInstance : ItemInstance
    {
        public SkillItemInstance(SkillItem itemType) : base(itemType)
        {
            
        }
    }
}