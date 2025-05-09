using System.Collections;
using System.Collections.Generic;
using Orby.Interfaces;
using UnityEngine;

namespace Orby.Enemy
{
    public class EnemyRunner : EnemyBase, IMoveable
    {
        private Vector3 direction;

        public Vector3 Direction { get => direction; set => direction = value; }

        public LayerMask endWayLayer; // End of the runner way

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            Move();   
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(1);
                Kill();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == endWayLayer)
            {
                Kill();
            }
        }

        public override void Kill()
        {
            Destroy(gameObject);
        }

        public void Move()
        {
            transform.Translate(Direction * Time.deltaTime);
        }
    }
}
