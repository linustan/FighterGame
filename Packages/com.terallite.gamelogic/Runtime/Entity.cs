namespace Packages.com.terallite.gamelogic.Runtime
{
    public enum EntityType
    {
        Fighter,
        Laser
    }

    public enum EntityTeam
    {
        Ally,
        Enemy
    }

    // Abstract base class for all game objects in the simulation.
    // Provides common functionality for position, heading, team assignment, and lifecycle management.
    // Part of the core entity framework forming the game's model layer.
    public abstract class Entity
    {
        public delegate void DespawnHandler(Entity entity);
        public abstract EntityType Type { get; }
        public Vector2 Position { get; protected set; }
        public Vector2 Heading { get; protected set; }
        public bool FlaggedForDeletion { get; private set; }
        public virtual Collider Collider => null;
        public EntityTeam Team { get; protected set; }

        public object AssociatedObject { get; set; }
        public event DespawnHandler Despawned;

        public void FlagForDeletion()
        {
            FlaggedForDeletion = true;
        }

        internal void OnDespawn()
        {
            Despawned?.Invoke(this);
        }

        internal virtual void TakeDamage(int damageAmount) { }
        public abstract void Tick(float deltaTime);
    }
}