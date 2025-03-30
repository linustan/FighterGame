namespace Packages.com.terallite.gamelogic.Runtime
{
    public interface IDurable
    {
        // A value from 0f - 1f, inclusive, indicating how much Durability remains out of the notional maximum Durability of this entity.
        public float RelativeDurability { get; }
    }
}