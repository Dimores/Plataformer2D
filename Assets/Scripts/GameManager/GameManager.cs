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
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Player playerPrefab;

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
            SetCameraTarget(playerPrefab.GetComponent<Transform>());
        }
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, GetPlayerSpawnVector(), Quaternion.identity, null);
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
