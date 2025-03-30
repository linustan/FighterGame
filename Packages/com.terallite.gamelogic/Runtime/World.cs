using System;
using System.Collections.Generic;

namespace Packages.com.terallite.gamelogic.Runtime
{
    public class World
    {
        public delegate void SpawnHandler(object sender, Entity entity);

        private readonly List<Fighter> _alliedFighters = new();
        private readonly CollisionSystem _collisionSystem = new();
        private readonly List<Fighter> _enemyFighters = new();
        private EnemyCommander _enemyCommander;

        private List<Entity> Entities { get; } = new();
        private List<Entity> SpawnedEntities { get; } = new();

        public event SpawnHandler Spawned;
        public event Action Won;
        public event Action Lost;

        public void Begin()
        {
            _alliedFighters.Add(new Fighter(new Vector2(-20, -10), new Vector2(0, 1), EntityTeam.Ally, HandleFireProjectile));
            Spawn(_alliedFighters[^1]);
            _alliedFighters.Add(new Fighter(new Vector2(20, -10), new Vector2(0, 1), EntityTeam.Ally, HandleFireProjectile));
            Spawn(_alliedFighters[^1]);
            _enemyFighters.Add(new Fighter(new Vector2(-40, 50), new Vector2(0, -1), EntityTeam.Enemy, HandleFireProjectile));
            Spawn(_enemyFighters[^1]);
            _enemyFighters.Add(new Fighter(new Vector2(0, 50), new Vector2(0, -1), EntityTeam.Enemy, HandleFireProjectile));
            Spawn(_enemyFighters[^1]);
            _enemyFighters.Add(new Fighter(new Vector2(40, 50), new Vector2(0, -1), EntityTeam.Enemy, HandleFireProjectile));
            Spawn(_enemyFighters[^1]);
            _enemyCommander = new EnemyCommander(_enemyFighters, _alliedFighters);
        }

        private void HandleFireProjectile(Fighter instigator, EntityType projectileType, Vector2 projectileHeading)
        {
            Spawn(new Laser(instigator, instigator.Position + projectileHeading * 15f, projectileHeading));
        }

        private void Spawn(Entity entity)
        {
            SpawnedEntities.Add(entity);
            if (entity.Collider != null)
            {
                _collisionSystem.Colliders.Add(entity.Collider);
            }
            Spawned?.Invoke(this, entity);
        }

        public void EndTurn()
        {
            _enemyCommander.UpdateCommands();
        }

        public void Tick(float deltaTime)
        {
            _collisionSystem.DetectCollisions();

            // Update entities first without spawning or de-spawning.
            foreach (var entity in Entities)
            {
                entity.Tick(deltaTime);
            }

            // Spawn any requested entities.
            Entities.AddRange(SpawnedEntities);
            SpawnedEntities.Clear();

            // Despawn entities flagged for deletion.
            foreach (var entity in Entities)
            {
                if (entity.FlaggedForDeletion)
                {
                    entity.OnDespawn();
                }
            }

            // Remove despawned entities from all lists.
            Entities.RemoveAll(e => e.FlaggedForDeletion);
            _alliedFighters.RemoveAll(e => e.FlaggedForDeletion);
            _enemyFighters.RemoveAll(e => e.FlaggedForDeletion);

            // Win / loss conditions
            if (_alliedFighters.Count == 0)
            {
                Lost?.Invoke();
            }
            else if (_enemyFighters.Count == 0)
            {
                Won?.Invoke();
            }
        }
    }
}