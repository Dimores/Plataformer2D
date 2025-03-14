using UnityEngine;

namespace Orby.Interfaces
{
    public interface IMoveable
    {
        public Rigidbody2D rb { get; set; }

        public void Move(float movementSpeed);
    }
}
