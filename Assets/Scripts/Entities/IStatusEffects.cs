namespace Entities
{
    public interface IStatusEffect
    {
        /// <summary>
        /// Called When the status effect is applied
        /// </summary>
        public void Start(EntityBody body);

        public void Tick(EntityBody body);
        
    }
}
// todo