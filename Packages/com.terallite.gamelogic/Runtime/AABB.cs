namespace Packages.com.terallite.gamelogic.Runtime
{
    // Axis-aligned bounding box for fast collision culling.
    public readonly struct AABB
    {
        private readonly Vector2 _topLeft;
        private readonly Vector2 _bottomRight;

        public AABB(Vector2 topLeft, Vector2 bottomRight)
        {
            _topLeft = topLeft;
            _bottomRight = bottomRight;
        }

        public static bool Overlaps(AABB lhs, AABB rhs)
        {
            // Check if one AABB is to the left of the other
            if (lhs._bottomRight.x < rhs._topLeft.x || rhs._bottomRight.x < lhs._topLeft.x)
                return false;

            // Check if one AABB is above the other
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (lhs._bottomRight.y > rhs._topLeft.y || rhs._bottomRight.y > lhs._topLeft.y)
                return false;

            // If neither of the above conditions is true, the AABBs overlap
            return true;
        }
    }
}