using UnityEngine;

namespace Orby.Interfaces
{
    public interface IMoveable
    {
        public Rigidbody2D Rb { get; set; }

        public void Move(float movementSpeed, float direction);
    }
}
