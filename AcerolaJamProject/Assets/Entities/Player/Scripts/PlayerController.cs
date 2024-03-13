using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Cinemachine;
using Combat;

namespace Player
{
    public class PlayerController : MonoBehaviour, IGrounded
    {
        private Rigidbody _rb;
        private IInvincibility _invincibility;
        [SerializeField] private CinemachineVirtualCamera _vcam;

        [SerializeField] private Transform _camTransform;
        [SerializeField] private Transform _headTransform;
        [SerializeField] private float _headTilt = 10;
        [SerializeField] private float _dashFOVMultiplier = 0.9f;
        [SerializeField] private float _fovChangeTime = 0.05f;
        private float _fov;
        private Vector2 _moveDir;
        [SerializeField] private float _speed = 5;
        [SerializeField] private float _retainedSpeed = 50;
        [SerializeField] private float _maxSpeed = 10;
        [Space]
        [SerializeField] private float _jumpForce = 20;
        [SerializeField] private float _dashJumpX = 100;
        [SerializeField] private float _dashJumpY = 70;
        [Space]
        [SerializeField] private float _dashTime = 0.2f;
        private float _dashTimer;
        [SerializeField] private float _dashVel;
        private bool _dashing => _dashTimer < _dashTime;
        private Vector3 _dashDir;
        [Space]
        [SerializeField] private float _slamSpeed = 20;
        private bool _slamming;
        [Space]
        [SerializeField] private Transform _groundCheckPos;
        public float gravityScale = 60;
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] private float _groundCheckRange = 0.3f;
        [SerializeField] private bool _grounded;
        public UnityEvent onGrounded;
        public bool IsGrounded => _grounded;

        public int dashCount = 3;
        private int _maxDash = 3;
        private float _dashRegenTime = 2;
        private float _dashRegenTimer;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _invincibility = GetComponent<IInvincibility>();
            _fov = _vcam.m_Lens.FieldOfView;
        }

        private void Update()
        {
            Turn();

            if (_dashing)
            {
                _vcam.m_Lens.FieldOfView = Mathf.Lerp(_vcam.m_Lens.FieldOfView, _fov * _dashFOVMultiplier, _fovChangeTime * Time.deltaTime);
                _dashTimer += Time.deltaTime;
                if (!_dashing)
                {
                    _rb.velocity = Vector3.zero;
                }
            }
            else if (_vcam.m_Lens.FieldOfView != 30) // This is a stupid fix for adding temporary zoom ------ REMOVE LATER ------
            {
                _vcam.m_Lens.FieldOfView = Mathf.Lerp(_vcam.m_Lens.FieldOfView, _fov, _fovChangeTime * Time.deltaTime);
            }

            if (_dashRegenTimer < _dashRegenTime && dashCount < _maxDash)
            {
                _dashRegenTimer += Time.deltaTime;
                if (_dashRegenTimer >= _dashRegenTime)
                {
                    dashCount++;
                    _dashRegenTimer = 0;
                }
            }
        }

        private void FixedUpdate()
        {
            Gravity();
            CheckGrounded();
            Move();
            if (_dashing)
            {
                _rb.velocity = _dashDir.normalized * _dashVel;
            }
            if (_slamming)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, -_slamSpeed, _rb.velocity.z);
                if (_grounded)
                {
                    _slamming = false;
                }
            }
            //Debug.Log(_rb.velocity.magnitude);
        }

        private void Turn()
        {
            Vector3 lookDir = new Vector3(_camTransform.forward.x, 0, _camTransform.forward.z);

            transform.rotation = Quaternion.LookRotation(lookDir.normalized, transform.up);

            if (Mathf.Abs(Vector3.Angle(_camTransform.forward, transform.forward)) < 80)
            {
                _headTransform.rotation = Quaternion.Euler(new Vector3(_headTransform.rotation.eulerAngles.x, _headTransform.rotation.eulerAngles.y, Mathf.Round(_moveDir.x) * -_headTilt));
            }
            else
            {
                _headTransform.rotation = Quaternion.Euler(new Vector3(_headTransform.rotation.eulerAngles.x, _headTransform.rotation.eulerAngles.y, 0));
            }
        }

        private void Gravity()
        {
            _rb.AddForce(-transform.up * gravityScale);
        }

        private void Move()
        {
            Vector2 moveDir = _moveDir.normalized;
            Vector3 dir = (transform.forward * moveDir.y) + (transform.right * moveDir.x);

            if (new Vector2(_rb.velocity.x, _rb.velocity.z).magnitude > _maxSpeed)
            {
                _rb.AddForce(dir * _retainedSpeed);
                return;
            }

            dir.y = 0;
            dir.Normalize();

            _rb.AddForce(dir.normalized * _speed);
        }

        private void StartDash()
        {
            dashCount--;
            _dashTimer = 0;
            _dashDir = ((transform.forward * _moveDir.normalized.y) + (transform.right * _moveDir.normalized.x)).normalized;
            if (_moveDir == Vector2.zero)
            {
                _dashDir = transform.forward;
            }
            _slamming = false;
        }

        private void Jump()
        {
            _rb.AddForce(transform.up * _jumpForce);
        }
        private void DashJump()
        {
            Vector3 force = (_dashDir * _dashJumpX) + (transform.up * _dashJumpY);
            _rb.AddForce(force);
            _dashTimer = _dashTime;
        }

        private void Kick()
        {

        }
        private void Slam()
        {
            _slamming = true;
        }

        private void CheckGrounded()
        {
            bool prevFrame = _grounded;
            _grounded = Physics.OverlapSphere(_groundCheckPos.position, _groundCheckRange, _groundLayers).Length > 0;

            if (_grounded && !prevFrame)
            {
                onGrounded.Invoke();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDir = context.ReadValue<Vector2>();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && _grounded)
            {
                if (_dashing)
                {
                    DashJump();
                }
                else
                {
                    Jump();
                }
            }
        }
        public void OnDash(InputAction.CallbackContext context)
        {
            if (_dashing)
                return;
            if (dashCount < 1)
                return;
            if (context.started)
            {
                _invincibility.BecomeInvincible(_dashTime);
                StartDash();
            }
        }
        public void OnKick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (IsGrounded)
                {
                    Kick();
                }
                else
                {
                    Slam();
                }
            }
        }

        public void SetGravity(float gravity)
        {
            gravityScale = gravity;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(_groundCheckPos.position, _groundCheckRange);
        }
    }
}