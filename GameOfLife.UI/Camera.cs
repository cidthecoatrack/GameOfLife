using System;
using System.Windows.Media.Media3D;
using MathNet.Numerics;

namespace GameOfLife.UI
{
    public class Camera
    {
        public readonly Vector3D Target;

        public Vector3D Position { get; private set; }
        public Vector3D Direction { get; private set; }
        public Vector3D Up { get; private set; }
        public Vector3D Right { get; private set; }

        public Camera()
        {
            Position = new Vector3D(0, 0, 0);
            Target = new Vector3D(0, 0, 0);
            Direction = new Vector3D(0, 0, -1);
            Up = new Vector3D(0, 1, 0);
            Right = new Vector3D(1, 0, 0);
        }

        public void SetZPosition(Double z)
        {
            Position = new Vector3D(Position.X, Position.Y, z);
        }

        public void MoveForward(Single velocity)
        {
            Position += GetUnitVector(Direction) * velocity;
            UpdateSettings();
        }

        private Vector3D GetUnitVector(Vector3D vector)
        {
            var divider = Math.Max(Math.Max(Math.Abs(vector.X), Math.Abs(vector.Y)), Math.Abs(vector.Z));
            return new Vector3D(vector.X / divider, vector.Y / divider, vector.Z / divider);
        }

        public void Rotate(Vector3D rotationAmount)
        {
            var rotationAroundX = new Matrix3D();
            rotationAroundX.Rotate(new Quaternion(Right, Trig.DegreeToRadian(rotationAmount.Y)));

            var rotationAroundY = new Matrix3D();
            rotationAroundY.Rotate(new Quaternion(Up, Trig.DegreeToRadian(rotationAmount.X)));

            var rotationMatrix = Matrix3D.Multiply(rotationAroundX, rotationAroundY);
            Up = Vector3D.Multiply(Up, rotationMatrix);

            var rotatedDirection = Vector3D.Multiply(Direction, rotationMatrix);
            Position += Direction - (rotatedDirection);

            UpdateSettings();
        }

        private void UpdateSettings()
        {
            Direction = Target - Position;
            Direction.Normalize();

            Up.Normalize();

            Right = Vector3D.CrossProduct(Direction, Up);
            Right.Normalize();
        }
    }
}