using UnityEngine;
using Orby.Player;
using Orby.Interfaces;

namespace Orby.Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable, IKillable
    {
        [Header("Enemy data")]
        [SerializeField] protected EnemyData enemyData;

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }

        protected virtual void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetLife();
            SetAnimator();
        }

        public void SetLife()
        {
            CurrentHealth = enemyData.life;
        }

        public void SetAnimator()
        {
            enemyData.characterAnimator = GetComponent<Animator>();
        }

        public void Damage(int damageAmount)
        {
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
                Kill();
        }

        public virtual void Kill() { }
    }
}
