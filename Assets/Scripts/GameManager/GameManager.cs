using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using DG.Tweening;
using Cinemachine;
using Orby.Player;

public class GameManager : Singleton<GameManager>
{
    //[Header("Enemies")]
    //public List<GameObject> enemies;

    #region REFERENCES
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [Space(5)]
    [SerializeField] private Transform playerSpawnPosition;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    #endregion


    private void Awake()
    {
        if (playerPrefab != null)
        {
            SpawnPlayer();
            SetCameraTarget(player.GetComponent<Transform>());
        }
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, GetPlayerSpawnVector(), Quaternion.identity, null);
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
