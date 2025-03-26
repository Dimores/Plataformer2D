using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby.Gun
{
    using Orby.Managers;
    using Orby.Player;

    public class GunBase : MonoBehaviour
    {
        [Header("Projectile")]
        public ProjectileBase prefabProjectile;

        [Header("Projectile Spawn Points")]
        public Transform shootPointDefault;
        public Transform shootPointDiagonal;
        public Transform shootPointUp;

        [Header("Animation triggers")]
        public Animator gunAnimator;
        public string defaultTrigger;
        public string upTrigger;
        public string diagonalTrigger;
        public string muzzleTrigger = "Muzzle";

        [Header("Audio")]
        public AudioRandomPlayAudioClips randomShootAudio;

        private Coroutine _currentCoroutine;
        private Player _player;
        private Transform _playerSideReference;
        private bool _wasShooting;
        private float _lastShootTime;

        private void Awake()
        {
            _playerSideReference = GetComponentInParent<Player>().gameObject.transform;
            _player = GetComponentInParent<Player>();
        }

        private void Update()
        {
            if (Input.GetKey(_player.playerData.attackKey))
            {
                TryStartShooting();
            }
            else if (Input.GetKeyUp(_player.playerData.attackKey))
            {
                StopShooting();
            }
        }

        private void TryStartShooting()
        {
            if (_currentCoroutine == null && Time.time >= _lastShootTime + _player.playerData.attackSpeed)
            {
                _wasShooting = true;
                _currentCoroutine = StartCoroutine(StartShoot());
            }
        }

        private void StopShooting()
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                _player.playerData.characterAnimator.SetTrigger(defaultTrigger);
                _wasShooting = false;
                _currentCoroutine = null;
            }
        }

        IEnumerator StartShoot()
        {
            while (true)
            {
                Shoot();
                _lastShootTime = Time.time; 
                yield return new WaitForSeconds(_player.playerData.attackSpeed);
            }
        }

        public void Shoot()
        {
            if (randomShootAudio != null) randomShootAudio.PlayRandom();
            gunAnimator.SetTrigger(muzzleTrigger);
            AudioManager.Instance.PlayAudioByTypeWithRandomPitch(AudioManager.AudioType.SHOOT,
                new Vector2(0.9f, 1.1f), 0.5f);

            var shootPoint = GetShootPoint();
            var direction = GetProjectileDirection();

            var projectile = Instantiate(prefabProjectile);
            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;
            projectile.side = _playerSideReference.transform.localScale.x;
            projectile.Direction = direction * _player.playerData.projectileSpeed;
            projectile.DamageAmount = _player.playerData.damage;
        }

        private Transform GetShootPoint()
        {
            if (InputManager.Instance.Horizontal != 0 && InputManager.Instance.Vertical > 0)
            {
                _player.playerData.characterAnimator.SetTrigger(diagonalTrigger);
                return shootPointDiagonal;
            }

            if (InputManager.Instance.Vertical > 0)
            {
                _player.playerData.characterAnimator.SetTrigger(upTrigger);
                return shootPointUp;
            }

            _player.playerData.characterAnimator.SetTrigger(defaultTrigger);
            return shootPointDefault;
        }

        private Vector3 GetProjectileDirection()
        {
            if (InputManager.Instance.Vertical > 0 && InputManager.Instance.Horizontal == 0)
                return Vector3.up;

            if (InputManager.Instance.Vertical > 0 && InputManager.Instance.Horizontal != 0)
            {
                return new Vector3(InputManager.Instance.Horizontal, 1, 0).normalized * Mathf.Sqrt(2);
            }

            return new Vector3(1f, 0f, 0f);
        }

        private void OnDisable()
        {
            StopShooting();
        }

        private void OnEnable()
        {
            if (_wasShooting && _currentCoroutine == null)
            {
                TryStartShooting();
            }
        }
    }
}
