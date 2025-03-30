namespace Packages.com.terallite.gamelogic.Runtime
{
    public class Laser : Entity
    {
        private readonly ColliderBox _collider;
        private readonly Entity _owner;
        private float _lifetime = 5f;

        public Laser(Entity owner, Vector2 position, Vector2 heading)
        {
            _owner = owner;
            Team = owner.Team;
            Position = position;
            Heading = heading;
            _collider = new ColliderBox(this, new BoundingBox(Position, 2f, 20f, Heading));
            _collider.Overlapped += ColliderOnOverlapped;
        }
        public override EntityType Type => EntityType.Laser;
        public override Collider Collider => _collider;

        private void ColliderOnOverlapped(Entity overlappingEntity)
        {
            // TODO: Rather than checking the type of the overlappingEntity, query its properties (e.g. isSolid) to determine if
            //       a laser will be absorbed by it.
            if (overlappingEntity != _owner && overlappingEntity is not Laser)
            {
                FlagForDeletion();
                // TODO: Expose parameters like damage via an Editor panel or similar so that they can be tuned
                //       easily and on-the-fly.
                overlappingEntity.TakeDamage(15);
            }
        }

        public override void Tick(float deltaTime)
        {
            Position += Heading * 80.0f * deltaTime;
            _collider.SetTransform(Position, Heading);
            _lifetime -= deltaTime;
            if (_lifetime <= 0f)
            {
                FlagForDeletion();
            }
        }
    }
}