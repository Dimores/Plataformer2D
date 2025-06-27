using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orby.Core.Singleton;
using DG.Tweening;
using Cinemachine;
using Orby.Player;
using Orby.Enemy.Boss;
using NUnit;
using System.Net;
using UnityEngine.Audio;

namespace Orby.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        //[Header("Enemies")]
        //public List<GameObject> enemies;

        #region REFERENCES
        [Header("Player")]
        [SerializeField] private GameObject player;

        [Space(5)]
        [SerializeField] private Transform playerSpawnPosition;

        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [Header("Boss")]
        public Boss boss;

        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private float abafadoCutoff = 500f;
        [SerializeField] private float normalCutoff = 22000f;

        [Header("Screens")]
        [SerializeField] private GameObject lostScreen;
        [SerializeField] private GameObject victoryScreen;
        [SerializeField] private RectTransform lostImageRect;


        public GameObject Player { get => player; set => player = value; }
        #endregion

        private bool gameOver = false;
        public bool GameOver
        {
            get => gameOver;
            set
            {
                gameOver = value;
                if (gameOver)
                {
                    MuffleMusic();
                    lostScreen.SetActive(true);

                    lostImageRect.anchoredPosition = new Vector2(lostImageRect.anchoredPosition.x, -1004);

                    lostImageRect.DOAnchorPosY(0f, 2f).SetEase(Ease.OutElastic);

                }
                else
                {
                    ResetMusic();
                }
            }
        }

        private void Start()
        {
            if (player != null)
            {
                SpawnPlayer();
                SetCameraTarget(Player.GetComponent<Transform>());
            }
        }

        private void SpawnPlayer()
        {
            Player = Instantiate(player, GetPlayerSpawnVector(), Quaternion.identity, null);
        }

        private Vector3 GetPlayerSpawnVector()
        {
            if (playerSpawnPosition != null)
                return playerSpawnPosition.position;
            else return Vector3.zero;
        }

        #region MUSIC
        private void MuffleMusic()
        {
            audioMixer.SetFloat("LowPassCutoff", abafadoCutoff);
        }

        private void ResetMusic()
        {
            audioMixer.SetFloat("LowPassCutoff", normalCutoff);
        }
        #endregion

        #region CAMERA
        private void SetCameraTarget(Transform cameraTarget)
        {
            if (virtualCamera != null)
                virtualCamera.Follow = cameraTarget;
        }
        #endregion
    }
}