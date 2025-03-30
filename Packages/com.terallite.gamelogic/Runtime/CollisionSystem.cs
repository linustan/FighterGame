using System.Collections.Generic;

namespace Packages.com.terallite.gamelogic.Runtime
{
    // Manages collision detection between game entities.
    // Uses a simple O(n²) algorithm to detect overlaps between colliders.
    // Part of the physics infrastructure for the game logic layer.
    public class CollisionSystem
    {
        public List<Collider> Colliders { get; } = new();


        public void DetectCollisions()
        {
            Colliders.RemoveAll(c => c.ParentEntityFlaggedForDeletion);
            foreach (var collider in Colliders)
            {
                foreach (var otherCollider in Colliders)
                {
                    // TODO: If this gets slow, we should use spatial partitioning (QuadTree or other) to 
                    //       reduce the number of candidate colliders.
                    if (collider != otherCollider && AABB.Overlaps(collider.Extents, otherCollider.Extents))
                    {
                        if (collider.Overlaps(otherCollider))
                        {
                            collider.HandleOverlap(otherCollider);
                        }
                    }
                }
            }
        }
    }
}