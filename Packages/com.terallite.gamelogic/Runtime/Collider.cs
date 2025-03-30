namespace Packages.com.terallite.gamelogic.Runtime
{
    public abstract class Collider
    {
        public delegate void OverlapHandler(Entity overlappingEntity);
        public abstract AABB Extents { get; }
        protected Entity ParentEntity { get; set; }
        internal bool ParentEntityFlaggedForDeletion => ParentEntity.FlaggedForDeletion;
        public event OverlapHandler Overlapped;
        public abstract bool Overlaps(Collider collider);
        internal abstract bool OverlapImpl(ColliderBox colliderBox);

        internal void HandleOverlap(Collider overlappingCollider)
        {
            Overlapped?.Invoke(overlappingCollider.ParentEntity);
        }
    }
}