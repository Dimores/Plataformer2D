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
        public GameObject shootPoints;
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
        private bool _wasShooting;
        private float _lastShootTime;

        private string _currentTrigger;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void Update()
        {
            if (Input.GetButton(_player.playerData.attackKey))
            {
                TryStartShooting();
            }
            else if (Input.GetButtonUp(_player.playerData.attackKey))
            {
                StopShooting();
            }
        }

        public void Aim()
        {
            if (Mathf.Abs(InputManager.Instance.Horizontal) > 0.1f)
            {
                _player.playerData.HandleScaleX(); 
            }

            if (InputManager.Instance.Horizontal != 0 && InputManager.Instance.Vertical > 0)
            {
                SetAnimationTriggerOnce(diagonalTrigger);
                return;
            }

            if (InputManager.Instance.Vertical > 0)
            {
                SetAnimationTriggerOnce(upTrigger);
                return;
            }

            SetAnimationTriggerOnce(defaultTrigger);
        }

        public void ExitAim()
        {
            SetAnimationTriggerOnce(defaultTrigger);
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
                new Vector2(1.1f, 1.4f), _player.playerData.normalShootVolume);

            FlipShootPoints();

            var shootPoint = GetShootPoint();
            var direction = GetProjectileDirection();

            var projectile = Instantiate(prefabProjectile);
            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;
            projectile.side = _player.playerData.direction;
            projectile.Direction = direction * _player.playerData.projectileSpeed;
            projectile.DamageAmount = _player.playerData.damage;
        }

        private Transform GetShootPoint()
        {
            if (InputManager.Instance.Horizontal != 0 && InputManager.Instance.Vertical > 0)
            {
                SetAnimationTrigger(diagonalTrigger);
                return shootPointDiagonal;
            }

            if (InputManager.Instance.Vertical > 0)
            {
                SetAnimationTrigger(upTrigger);
                return shootPointUp;
            }

            SetAnimationTrigger(defaultTrigger);
            return shootPointDefault;
        }


        private void SetAnimationTriggerOnce(string newTrigger)
        {
            if (_currentTrigger == newTrigger) return;

            if (!string.IsNullOrEmpty(_currentTrigger))
                _player.playerData.characterAnimator.ResetTrigger(_currentTrigger);

            _player.playerData.characterAnimator.SetTrigger(newTrigger);
            _currentTrigger = newTrigger;
        }

        public void SetAnimationTrigger(string trigger)
        {
            _player.playerData.characterAnimator.SetTrigger(trigger);
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

        // Flip shoot points and angle (left side) correctly
        private void FlipShootPoints()
        {
            shootPoints.transform.localPosition = _player.playerData.direction == 1 ?
                Vector3.zero : new Vector3(1.41f, 0f, 0f);

            shootPoints.transform.localScale = new Vector3(_player.playerData.direction, 
                shootPoints.transform.localScale.y, shootPoints.transform.localScale.z);
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
