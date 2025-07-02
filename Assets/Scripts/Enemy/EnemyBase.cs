using UnityEngine;
using Orby.Player;
using Orby.Interfaces;
using System.Collections.Generic;
using Orby.Interfaces.General;
using System;

namespace Orby.Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable, IKillable
    {
        [Header("Enemy data")]
        [SerializeField] protected EnemyData enemyData;

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public Transform SpawnPosition { get; set; }

        protected Animator animator;
        public Animator Animator { get => animator; private set => animator = value; }

        public List<SpriteRenderer> SpriteRenderers { get; private set; } = new List<SpriteRenderer>();

        public static event Action<EnemyBase> OnEnemyKilled;


        private void Awake()
        {
            FillSpriteRendererList();
        }

        protected virtual void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetLife();
            SetAnimator();
            FillSpriteRendererList();
        }

        private void FillSpriteRendererList()
        {
            SpriteRenderers.Clear();
            SpriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
        }

        public void SetLife()
        {
            CurrentHealth = enemyData.life;
        }

        public void SetAnimator()
        {
            Animator = GetComponent<Animator>();
        }

        public void Damage(int damageAmount)
        {
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
                Kill();
        }

        public virtual void Kill() {
            OnEnemyKilled?.Invoke(this);
        }
    }
}
