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
            SetLife();
        }

        public void SetLife()
        {
            CurrentHealth = enemyData.life;
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
