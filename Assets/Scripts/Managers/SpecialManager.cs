using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Orby.Core.Singleton;
using Orby.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Orby.Managers
{
    public class SpecialManager : Singleton<SpecialManager>
    {
        [Header("Player Data")]
        [SerializeField] private PlayerData playerData;

        [Header("UI Setup")]
        public List<Image> bullets;

        private void Start()
        {
            playerData.ResetSpecial();
        }

        public void AddSpecial(int value)
        {
            playerData.AddSpecial(value);
            UpdateSpecialUI();
        }

        public void RemoveSpecial(int value)
        {
            playerData.RemoveSpecial(value);
            UpdateSpecialUI();
        }

        private void UpdateSpecialUI()
        {
            float maxSpecial = 400f;
            float specialPerBullet = maxSpecial / bullets.Count;

            for (int i = 0; i < bullets.Count; i++)
            {
                float bulletMin = i * specialPerBullet;
                float bulletMax = (i + 1) * specialPerBullet;

                float targetFill = Mathf.InverseLerp(bulletMin, bulletMax, playerData.specialAmount);
                targetFill = Mathf.Clamp01(targetFill);

                Image bullet = bullets[i];

                bullet.DOFillAmount(targetFill, 0.3f)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        Color targetColor = (bullet.fillAmount >= 1f) ? Color.white : Color.black;
                        bullet.DOColor(targetColor, 0.5f).SetEase(Ease.InOutSine);
                    });
            }
        }


    }
}
