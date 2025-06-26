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
        public GameObject shootPointsEnd;
        public Transform shootPointDefault;
        public Transform shootPointDiagonal;
        public Transform shootPointDiagonalDown;
        public Transform shootPointUp;
        public Transform shootPointDown;
        public Transform shootPointDefaultEnd;
        public Transform shootPointDiagonalEnd;
        public Transform shootPointDiagonalDownEnd;
        public Transform shootPointUpEnd;
        public Transform shootPointDownEnd;

        [Header("Animation triggers")]
        public Animator gunAnimator;
        public string defaultTrigger;
        public string upTrigger;
        public string downTrigger;
        public string diagonalTrigger;
        public string diagonalDownTrigger;
        public string muzzleTrigger = "Muzzle";

        [Header("Audio")]
        public AudioRandomPlayAudioClips randomShootAudio;

        private Coroutine _currentCoroutine;
        private Player _player;
        private bool _wasShooting;
        private float _lastShootTime;

        private string _currentTrigger;

        private Transform _lastShootPoint;


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

            if (InputManager.Instance.Horizontal != 0 && InputManager.Instance.Vertical < 0)
            {
                SetAnimationTriggerOnce(diagonalDownTrigger);
                return;
            }

            if (InputManager.Instance.Vertical > 0)
            {
                SetAnimationTriggerOnce(upTrigger);
                return;
            }

            if (InputManager.Instance.Vertical < 0)
            {
                SetAnimationTriggerOnce(downTrigger);
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
                _player.animator.SetTrigger(defaultTrigger);
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
            FlipSprite(projectile.GetComponentInChildren<SpriteRenderer>());
            projectile.transform.position = shootPoint.position;
            projectile.transform.rotation = shootPoint.rotation;
            projectile.side = _player.playerData.direction;
            projectile.Direction = direction * _player.playerData.projectileSpeed;
            projectile.DamageAmount = _player.playerData.damage;
        }

        public Transform GetShootPoint()
        {
            float h = InputManager.Instance.Horizontal;
            float v = InputManager.Instance.Vertical;

            if (Mathf.Abs(h) > 0.1f && v < -0.1f)
            {
                SetAnimationTrigger(diagonalDownTrigger);
                _lastShootPoint = shootPointDiagonalDown;
                return shootPointDiagonalDown;
            }

            if (Mathf.Abs(h) > 0.1f && v > 0.1f)
            {
                SetAnimationTrigger(diagonalTrigger);
                _lastShootPoint = shootPointDiagonal;
                return shootPointDiagonal;
            }

            if (v > 0.1f)
            {
                SetAnimationTrigger(upTrigger);
                _lastShootPoint = shootPointUp;
                return shootPointUp;
            }

            if (v < -0.1f)
            {
                SetAnimationTrigger(downTrigger);
                _lastShootPoint = shootPointDown;
                return shootPointDown;
            }

            SetAnimationTrigger(defaultTrigger);
            _lastShootPoint = shootPointDefault;
            return shootPointDefault;
        }

        public Transform GetEndShootPoint()
        {
            if (_lastShootPoint == shootPointDiagonalDown) return shootPointDiagonalDownEnd;
            if (_lastShootPoint == shootPointDiagonal) return shootPointDiagonalEnd;
            if (_lastShootPoint == shootPointUp) return shootPointUpEnd;
            if (_lastShootPoint == shootPointDown) return shootPointDownEnd;
            return shootPointDefaultEnd;
        }

        private void SetAnimationTriggerOnce(string newTrigger)
        {
            if (_currentTrigger == newTrigger) return;

            if (!string.IsNullOrEmpty(_currentTrigger))
                _player.animator.ResetTrigger(_currentTrigger);

            _player.animator.SetTrigger(newTrigger);
            _currentTrigger = newTrigger;
        }

        public void SetAnimationTrigger(string trigger)
        {
            _player.animator.SetTrigger(trigger);
        }

        private Vector3 GetProjectileDirection()
        {
            var h = InputManager.Instance.Horizontal;
            var v = InputManager.Instance.Vertical;

            if (v > 0 && h == 0)
                return Vector3.up;

            if (v > 0 && h != 0)
                return new Vector3(h, 1, 0).normalized;

            if (v < 0 && h == 0)
                return Vector3.down;

            if (v < 0 && h != 0)
                return new Vector3(h, -1, 0).normalized;

            return new Vector3(1f, 0f, 0f);
        }

        public void FlipShootPoints()
        {
            shootPoints.transform.localPosition = _player.playerData.direction == 1 ?
                Vector3.zero : new Vector3(1.41f, 0f, 0f);

            shootPointsEnd.transform.localPosition = _player.playerData.direction == 1 ?
                Vector3.zero : new Vector3(1.41f, 0f, 0f);

            shootPoints.transform.localScale = new Vector3(_player.playerData.direction,
                shootPoints.transform.localScale.y, shootPoints.transform.localScale.z);

            shootPointsEnd.transform.localScale = new Vector3(_player.playerData.direction,
                shootPoints.transform.localScale.y, shootPoints.transform.localScale.z);
        }

        private void FlipSprite(SpriteRenderer sprite)
        {
            sprite.flipX = _player.playerData.direction == 1 ? false : true;
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
