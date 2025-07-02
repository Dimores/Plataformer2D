using System;
using UnityEngine;

namespace Orby.Interfaces.IPhase
{
    public interface IPhase
    {
        event Action OnAttackFinished;
        event Action<Transform> OnSummonedEnemyKilled;

        void FirstAttack();
        void SecondAttack(Transform spawnPoint);
        bool CanUseSecondAttack();
    }
}
