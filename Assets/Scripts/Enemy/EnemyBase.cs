using UnityEngine;
using Orby.Player;

namespace Orby.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        public int damage = 10;
        public float pushForce;
        public float knockTime;

        public Animator anim;
        public string triggerAttack = "Attack";
        public string triggerDeath = "Death";

        public HealthBase healthBase;

        public float timeToDestroy;
        public AudioSource audioSourceKill;

        private Collider2D _collider;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();

            if (healthBase != null)
            {
                healthBase.OnKill += OnEnemyKill;
            }
        }

        private void OnEnemyKill()
        {
            healthBase.OnKill -= OnEnemyKill;

            if (_collider != null)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                _collider.enabled = false;
            }

            if (audioSourceKill != null) audioSourceKill.Play();
            PlayDeathAnimation();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var health = collision.gameObject.GetComponent<HealthBase>();
            //Player player = collision.gameObject.GetComponent<Player>();
            Rigidbody2D playerRigidBody = null;


            //if (player != null) playerRigidBody = player.GetComponent<Rigidbody2D>();

            if (health != null)
            {
                health.Damage(damage);
                if (playerRigidBody != null)
                {
                    //player.Knock(knockTime); 

                    float direction = Mathf.Sign(transform.localScale.x); // Garante que a direção está correta
                    Vector2 knockbackDirection = new Vector2(-direction, 1.5f); // Aumenta o valor Y
                    playerRigidBody.AddForce(knockbackDirection * pushForce, ForceMode2D.Impulse);
                }

                PlayAttackAnimation();
            }
        }

        private void PlayAttackAnimation()
        {
            anim.SetTrigger(triggerAttack);
        }

        private void PlayDeathAnimation()
        {
            anim.SetTrigger(triggerDeath);
        }

        public void Damage(int amount)
        {
            healthBase.Damage(amount);
        }

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
