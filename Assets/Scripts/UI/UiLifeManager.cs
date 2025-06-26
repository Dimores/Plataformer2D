using System.Collections;
using System.Collections.Generic;
using Orby.Core.Singleton;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Orby.UI
{
    public class UiLifeManager : Singleton<UiLifeManager>
    {
        [Header("References setup")]
        public List<GameObject> lifes;
        public GameObject lifeContainer;
        public GameObject deadContainer;

        [Header("DOTween setup")]
        public float scaleAmount = 1.2f;
        public float scaleDuration = .2f;
        public float scaleDownDuration = .1f;
        public Color fadeColor = Color.red;

        private Tween _fadeTween;

        public void UpdateLifeOnUi(int value)
        {
            if (value > 0)
            {
                for (int i = 0; i < lifes.Count; i++)
                {
                    lifes[i].SetActive(i == value - 1);
                }

                lifeContainer.transform.DOScale(scaleAmount, scaleDuration)
                    .OnComplete(() => lifeContainer.transform.DOScale(1f, scaleDownDuration));

                if (value == 1)
                {
                    ActivateFade();
                }
            }
            else
            {
                lifeContainer.SetActive(false);
                deadContainer.SetActive(true);
            }

        }

        private void ActivateFade()
        {
            _fadeTween?.Kill();

            Image lifeImage = lifeContainer.GetComponent<Image>();
            if (lifeImage == null) return;

            _fadeTween = lifeImage.DOColor(Color.red, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}
