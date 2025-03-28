using System.Collections;
using System.Collections.Generic;
using Orby.Core.Singleton;
using UnityEngine;
using DG.Tweening; 

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

        public void UpdateLifeOnUi(int value)
        {
            if (value > 0)
            {
                for (int i = 0; i < lifes.Count; i++)
                {
                    lifes[i].SetActive(i == value - 1);
                }

                lifeContainer.transform.DOScale(scaleAmount, scaleDuration)
                    .OnComplete(() => lifeContainer.transform.DOScale(1f, scaleDuration));
            }
            else
            {
                lifeContainer.SetActive(false);
                deadContainer.SetActive(true);
            }

        }
    }
}
