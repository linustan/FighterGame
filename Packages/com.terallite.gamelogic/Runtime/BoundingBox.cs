using System;

namespace Packages.com.terallite.gamelogic.Runtime
{
    public class BoundingBox
    {
        // The four corners of the bounding box
        private readonly Vector2[] _corners = new Vector2[4];
        // ForwardDir is calculated as the perpendicular to rightDir
        private Vector2 _forwardDir;

        // Direction vectors for the box's orientation
        // RightDir is the normalized direction of the "right" side of the box
        private Vector2 _rightDir;

        // Constructor for creating a bounding box with position, size, and orientation
        public BoundingBox(Vector2 center, float width, float height, Vector2 forward)
        {
            Width = width;
            Height = height;
            SetTransform(center, forward);
        }

        // The center point of the bounding box
        private Vector2 Center { get; set; }

        // The width and height of the bounding box
        private float Width { get; }
        private float Height { get; }

        // Update corner positions based on current center, size, and orientation vectors
        private void UpdateCorners()
        {
            // Calculate half-width and half-height for convenience
            var halfWidth = Width / 2f;
            var halfHeight = Height / 2f;

            // Calculate the four corners using the orientation vectors
            // For each corner, multiply the direction vector by the appropriate distance and add to center

            // Top-left: center - halfWidth*rightDir - halfHeight*upDir
            _corners[0] = Center - _rightDir * halfWidth - _forwardDir * halfHeight;

            // Top-right: center + halfWidth*rightDir - halfHeight*upDir
            _corners[1] = Center + _rightDir * halfWidth - _forwardDir * halfHeight;

            // Bottom-right: center + halfWidth*rightDir + halfHeight*upDir
            _corners[2] = Center + _rightDir * halfWidth + _forwardDir * halfHeight;

            // Bottom-left: center - halfWidth*rightDir + halfHeight*upDir
            _corners[3] = Center - _rightDir * halfWidth + _forwardDir * halfHeight;
        }

        // Set new properties and update corners
        public void SetTransform(Vector2 center, Vector2 forward)
        {
            Center = center;
            _forwardDir = forward;

            // Calculate the right direction as perpendicular to the forward direction
            _rightDir = new Vector2(_forwardDir.y, -_forwardDir.x);

            UpdateCorners();
        }

        // Extract an Axis-Aligned Bounding Box (AABB) that fully contains this oriented bounding box
        public AABB ToAABB()
        {
            // Initialize min and max coordinates with the first corner
            var minX = _corners[0].x;
            var minY = _corners[0].y;
            var maxX = _corners[0].x;
            var maxY = _corners[0].y;

            // Find the min and max x, y coordinates from all corners
            for (var i = 1; i < _corners.Length; i++)
            {
                var point = _corners[i];

                // Update min values
                if (point.x < minX) minX = point.x;
                if (point.y < minY) minY = point.y;

                // Update max values
                if (point.x > maxX) maxX = point.x;
                if (point.y > maxY) maxY = point.y;
            }

            // Create the AABB using the TopLeft and BottomRight points
            var topLeft = new Vector2(minX, maxY);
            var bottomRight = new Vector2(maxX, minY);

            return new AABB(topLeft, bottomRight);
        }


        // Check if this bounding box overlaps with another using the Separating Axis Theorem (SAT)
        public bool Overlaps(BoundingBox other)
        {
            // For OBBs, we need to check four axes: the two axes of this box and the two axes of the other box
            Span<Vector2> axes = stackalloc Vector2[4]
            {
                _rightDir,
                _forwardDir,
                other._rightDir,
                other._forwardDir
            };

            // For each axis, project both shapes onto it
            foreach (var axis in axes)
            {
                ProjectShapeOntoAxis(_corners, axis, out var min1, out var max1);
                ProjectShapeOntoAxis(other._corners, axis, out var min2, out var max2);

                // If projections don't overlap, the shapes are separable by this axis
                if (max1 < min2 || max2 < min1)
                {
                    return false; // Shapes are separable, no collision
                }
            }

            // If we get here, no separating axis was found, so the shapes overlap
            return true;
        }

        // Helper method to project a shape onto an axis
        private static void ProjectShapeOntoAxis(Vector2[] points, Vector2 axis, out float min, out float max)
        {
            // Project the first point
            var dot = points[0].Dot(axis);
            min = max = dot;

            // Project the rest of the point
            for (var i = 1; i < points.Length; i++)
            {
                dot = points[i].Dot(axis);

                if (dot < min)
                {
                    min = dot;
                }
                if (dot > max)
                {
                    max = dot;
                }
            }
        }
    }
}