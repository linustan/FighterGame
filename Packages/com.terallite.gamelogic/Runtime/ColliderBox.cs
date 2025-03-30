namespace Packages.com.terallite.gamelogic.Runtime
{
    public class ColliderBox : Collider
    {
        public ColliderBox(Entity parentEntity, BoundingBox boundingBox)
        {
            ParentEntity = parentEntity;
            BoundingBox = boundingBox;
        }

        public override AABB Extents => BoundingBox.ToAABB();
        private BoundingBox BoundingBox { get; }

        public override bool Overlaps(Collider collider)
        {
            return collider.OverlapImpl(this);
        }
        internal override bool OverlapImpl(ColliderBox colliderBox)
        {
            return colliderBox.BoundingBox.Overlaps(BoundingBox);
        }
        public void SetTransform(Vector2 position, Vector2 forward)
        {
            BoundingBox.SetTransform(position, forward);
        }
    }
}