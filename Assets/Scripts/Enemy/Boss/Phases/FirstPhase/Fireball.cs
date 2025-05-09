using System.Collections;
using System.Collections.Generic;
using Orby.Interfaces;
using Orby.Managers;
using UnityEngine;

namespace Orby.Enemy.Boss.Phases.FirstPhase
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private Vector3 direction;

        [SerializeField] private LayerMask groundLayer;

        public Vector3 Direction { get => direction; set => direction = value; }

        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        void Update()
        {
            transform.Translate(Direction * Time.deltaTime, Space.World);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                var damageable = collision.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                    damageable.Damage(1);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                var damageable = collision.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(1);
                    Destroy(gameObject);
                }
            }else if(collision.gameObject.tag == "Ground")
            {
                VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.EXPLOSION, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
