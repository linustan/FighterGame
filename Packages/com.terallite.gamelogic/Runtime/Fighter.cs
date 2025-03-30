namespace Packages.com.terallite.gamelogic.Runtime
{
    public class Fighter : Entity, IDurable
    {
        public delegate void FireProjectileHandler(Fighter instigator, EntityType projectileType, Vector2 projectileHeading);
        private const float LaserCooldownInitial = 0.5f;
        private const int MaxDurability = 100;
        private readonly ColliderBox _collider;
        private int _durability = MaxDurability;
        private float _laserCooldown = LaserCooldownInitial;

        public Fighter(Vector2 position, Vector2 heading, EntityTeam team, FireProjectileHandler fireProjectileHandler)
        {
            Position = position;
            Heading = heading;
            Team = team;
            _collider = new ColliderBox(this, new BoundingBox(Position, 10f, 10f, Heading));
            ProjectileFired += fireProjectileHandler;
        }

        public override EntityType Type => EntityType.Fighter;
        public override Collider Collider => _collider;

        public Vector2 Destination
        {
            set => Heading = (value - Position).Normalized();
        }

        public Vector2 LaserTarget { get; set; }

        public float RelativeDurability => _durability / (float)MaxDurability;

        public event FireProjectileHandler ProjectileFired;

        internal override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            _durability -= damageAmount;
            if (_durability <= 0)
            {
                FlagForDeletion();
            }
        }

        public override void Tick(float deltaTime)
        {
            Position += Heading * 10.0f * deltaTime;
            // TODO: Simply subtracting deltaTime from a cooldown like this will likely result in issues
            //       resulting from the accumulation of floating point errors, especially with high framerates.
            //       Refactor before embarking on the expansion of this game.
            _laserCooldown -= deltaTime;
            if (_laserCooldown <= 0f)
            {
                ProjectileFired?.Invoke(this, EntityType.Laser, (LaserTarget - Position).Normalized());
                _laserCooldown = LaserCooldownInitial;
            }
            _collider.SetTransform(Position, Heading);
        }
    }
}