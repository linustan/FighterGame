using System;
using System.Collections.Generic;

namespace Packages.com.terallite.gamelogic.Runtime
{
    public class EnemyCommander
    {
        private readonly List<Fighter> _myFighters;
        private readonly Random _random = new();
        private readonly List<Fighter> _theirFighters;

        internal EnemyCommander(List<Fighter> myFighters, List<Fighter> theirFighters)
        {
            _myFighters = myFighters;
            _theirFighters = theirFighters;
        }

        public void UpdateCommands()
        {
            if (_theirFighters.Count == 0) return;

            foreach (var fighter in _myFighters)
            {
                var targetPos = _theirFighters[0].Position;
                var targetDir = (targetPos - fighter.Position).Normalized();
                var sideways = new Vector2(-targetDir.y, targetDir.x) * 60f;
                var randomOffset = new Vector2((float)_random.NextDouble() * 2f - 1f, (float)_random.NextDouble() * 2f - 1f) * 20f;

                fighter.LaserTarget = _theirFighters[0].Position;
                fighter.Destination = sideways + randomOffset;
            }
        }
    }
}