using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orby.Core.Singleton;
using DG.Tweening;
using Cinemachine;
using Orby.Player;

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

        public GameObject Player { get => player; set => player = value; }
        #endregion


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

        #region CAMERA
        private void SetCameraTarget(Transform cameraTarget)
        {
            if (virtualCamera != null)
                virtualCamera.Follow = cameraTarget;
        }
        #endregion
    }
}