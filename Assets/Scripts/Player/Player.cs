using UnityEngine;
using DG.Tweening;
using Orby.Interfaces;
using Unity.VisualScripting;
using Orby.Player.StateMachine;
using Orby.Player.StateMachine.ConcretStates;

namespace Orby.Player
{
    public class Player : MonoBehaviour, IDamageable, IMoveable, IKillable
    {
        public PlayerData playerData;

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public Rigidbody2D Rb { get; set; }

        #region State Machine Variables
        public PlayerStateMachine StateMachine { get; set; }
        public PlayerIdleState IdleState { get; set; }
        public PlayerMovementState MovementState { get; set; }
        public PlayerJumpState JumpState { get; set; }
        public PlayerFallingState FallingState { get; set; }
        public PlayerDashState DashState { get; set; }
        #endregion

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine);
            MovementState = new PlayerMovementState(this, StateMachine);
            JumpState = new PlayerJumpState(this, StateMachine);
            FallingState = new PlayerFallingState(this, StateMachine);
            DashState = new PlayerDashState(this, StateMachine);
        }

        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();

            MaxHealth = playerData.life;
            CurrentHealth = MaxHealth;

            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentPlayerState.FrameUpdate();
        }


        public void Damage(int damageAmount)
        {
            CurrentHealth -= damageAmount;

            if (CurrentHealth <= 0)
                Kill();
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public void Move(float movementSpeed, float direction)
        {
            Rb.velocity = new Vector2(movementSpeed * direction, Rb.velocity.y);
        }

        public void Jump(float jumpForce)
        {
            Rb.velocity = Vector2.up * playerData.jumpForce;
        }

        #region AnimationTriggers
        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
        }

        public enum AnimationTriggerType {
            PlayerRun,
            PlayerJump
        }
        #endregion

        #region OLD
        //[Header("Player Data")]
        //public PlayerData playerData;

        //[Header("Ground Check")]
        //public Transform groundCheck;
        //public LayerMask groundLayer;
        //public float groundCheckRadius = 0.1f;

        //[Header("VFX")]
        //public Vector3 walkVFXOffset;
        //public Vector3 jumpVFXOffset;
        //public Vector3 fallVFXOffset;

        //[Header("Layer")]
        //public LayerMask collisionLayer;


        //private Rigidbody2D _myRigidbody;
        //private float _speedRun;
        //private float _currentSpeed;
        //private float _currentScaleX;
        //private Vector2 _friction = new Vector2(-0.1f, 0);
        //private bool _isGrounded = false; 
        //private bool _wasFalling = false;
        //private bool _canControl = true;
        //private bool _takeHit = false;

        //private HealthBase _healthBase;
        //private Animator _currentPlayer;

        //[SerializeField] private float _horizontal;

        //private ParticleSystem _walkVFX;

        //public bool CanControl { get => _canControl; set => _canControl = value; }

        //private void Awake()
        //{
        //    Init();
        //}

        //private void Init()
        //{
        //    _currentPlayer = Instantiate(playerData.player, transform);
        //    _myRigidbody = GetComponent<Rigidbody2D>();
        //    _healthBase = GetComponent<HealthBase>();
        //    _healthBase.OnKill += OnPlayerKill;
        //    _speedRun = playerData.speed.value + (playerData.speed.value * 0.5f);
        //    _currentScaleX = transform.localScale.x;
        //    _walkVFX = VFXManager.Instance.PlayAndGetPermanentVFXByType(VFXManager.VFXType.WALK,
        //        this.transform.position, walkVFXOffset, this.transform);
        //}

        //private void OnPlayerKill()
        //{
        //    _healthBase.OnKill -= OnPlayerKill;
        //    _currentPlayer.SetTrigger(playerData.triggerDeath.value);
        //    CanControl = false;
        //}

        //void Update()
        //{
        //    CheckGrounded();
        //    walkVFXControl();
        //    if (gameObject.activeInHierarchy && CanControl)
        //    {
        //        _currentPlayer.SetBool("Damage", false);
        //        HandleJump();
        //        HandleMovement();
        //        HandleScaleFall();
        //    } else if(!CanControl && _takeHit)
        //    {
        //        _currentPlayer.SetBool("Damage", true);
        //    }
        //}


        //// Function Called when take a hit from any enemy
        //public void Knock(float time)
        //{
        //    DisableControl();
        //    Invoke(nameof(EnableControl), time);
        //}

        //private void DisableControl()
        //{
        //    CanControl = false;
        //    _takeHit = true;
        //}

        //private void EnableControl()
        //{
        //    if (!_healthBase.IsDead)
        //        CanControl = true;
        //    _takeHit = false;
        //}

        //private void walkVFXControl()
        //{
        //    if (_walkVFX != null)
        //    {
        //        if (!_isGrounded)
        //        {
        //            _walkVFX.Stop();
        //        }
        //        else if(!_walkVFX.isPlaying)
        //        {
        //            _walkVFX.Play();
        //        }
        //    }
        //}

        //private void HandleMovement()
        //{
        //    _horizontal = Input.GetAxisRaw(playerData.moveAxis.value);

        //    if (Input.GetKey(playerData.run.value))
        //    {
        //        _currentSpeed = _speedRun;
        //        if (_horizontal == 0)
        //            _currentPlayer.SetBool(playerData.boolSprint.value, false);
        //        else
        //            _currentPlayer.SetBool(playerData.boolSprint.value, true);
        //    }
        //    else
        //    {
        //        _currentSpeed = playerData.speed.value;
        //        _currentPlayer.SetBool(playerData.boolSprint.value, false);
        //    }

        //    if (_horizontal < 0)
        //    {
        //        _myRigidbody.velocity = new Vector2(-_currentSpeed, _myRigidbody.velocity.y);

        //        if (_myRigidbody.transform.localScale.x != -1)
        //        {
        //            _myRigidbody.transform.DOScaleX(-1,
        //                playerData.playerSwipeDuration.value);
        //            _currentScaleX = -1;
        //        }

        //        _currentPlayer.SetBool(playerData.boolRun.value, true);
        //    }
        //    else if (_horizontal > 0)
        //    {
        //        _myRigidbody.velocity = new Vector2(_currentSpeed, _myRigidbody.velocity.y);

        //        if (_myRigidbody.transform.localScale.x != 1)
        //        {

        //            _myRigidbody.transform.DOScaleX(1,
        //                playerData.playerSwipeDuration.value);
        //            _currentScaleX = 1;
        //        }

        //        _currentPlayer.SetBool(playerData.boolRun.value, true);
        //    }
        //    else
        //    {
        //        _myRigidbody.velocity = new Vector2(0, _myRigidbody.velocity.y);
        //        _currentPlayer.SetBool(playerData.boolRun.value, false);
        //    }
        //}


        //private void setFriction()
        //{
        //    if (_myRigidbody.velocity.x > 0)
        //    {
        //        _myRigidbody.velocity += _friction;
        //    }
        //    else if (_myRigidbody.velocity.x < 0)
        //    {
        //        _myRigidbody.velocity -= _friction;
        //    }
        //}

        //private void PlayJumpVFX()
        //{
        //    VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.JUMP, transform.position, jumpVFXOffset);
        //}

        //private void PlayFallVFX()
        //{
        //    VFXManager.Instance.PlayVFXByType(VFXManager.VFXType.FALL, transform.position, fallVFXOffset);
        //}

        //private void HandleJump()
        //{
        //    if (Input.GetKeyDown(playerData.jump.value) && _isGrounded)
        //    {
        //        _myRigidbody.velocity = Vector2.up * 
        //            playerData.jumpForce.value;

        //        DOTween.Kill(_myRigidbody.transform);

        //        _myRigidbody.transform.localScale = new Vector3(_currentScaleX, 1, 1);

        //        _currentPlayer.SetTrigger(playerData.triggerJump.value);
        //        HandleScaleJump();
        //        PlayJumpVFX();
        //    }
        //}


        //private void HandleScaleJump()
        //{
        //    DOTween.Kill(_myRigidbody.transform);

        //    _myRigidbody.transform.DOScaleY(playerData.jumpScaleY.value,
        //        playerData.animationDuration.value)
        //        .SetLoops(2, LoopType.Yoyo)
        //        .SetEase(Ease.OutQuad);
        //}


        //private void HandleScaleFall()
        //{
        //    if (_wasFalling && _isGrounded)
        //    {
        //        _wasFalling = false;
        //        _currentPlayer.SetBool(playerData.boolFalling.value, false); 

        //        DOTween.Kill(_myRigidbody.transform);

        //        _myRigidbody.transform.DOScale(new Vector2(playerData.fallScaleX.value 
        //            * _currentScaleX,
        //            playerData.fallScaleY.value), 
        //            playerData.animationDuration.value / 2)
        //            .SetEase(Ease.InOutQuad)
        //            .OnComplete(() =>
        //            {
        //                _myRigidbody.transform.DOScale(new Vector2(1 * _currentScaleX, 1), 
        //                    playerData.animationDuration.value / 2)
        //                    .SetEase(Ease.OutBack);
        //            });
        //        PlayFallVFX();
        //    }

        //    if (!_wasFalling && !_isGrounded && _myRigidbody.velocity.y < -0.1f) 
        //    {
        //        _wasFalling = true;
        //        _currentPlayer.SetBool(playerData.boolFalling.value, true);
        //    }
        //}

        //private void CheckGrounded()
        //{
        //    _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        //}

        //private void OnDestroy()
        //{
        //    DOTween.Kill(transform);
        //} 
        #endregion
    }
}