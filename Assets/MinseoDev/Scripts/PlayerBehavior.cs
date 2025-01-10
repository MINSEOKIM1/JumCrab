using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // player's setting
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accel;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPower;

    [SerializeField] private float airSpeedDecrease;

    [SerializeField] private float setSpeedZeroOffset;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayers;
    
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private float jumpTimeOut;

    // player's current state
    public float _speed;

    public bool _grounded;
    public bool _isClimbing;

    public float _jumpTimeOutElapsed;

    public bool _isClimbingLeftWall;
    
    // components;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private PlayerInputManager _playerInput;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInputManager>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
        MoveHorizontally();
        ApplyActualMove();
    }

    private void Update()
    {
        _jumpTimeOutElapsed -= Time.deltaTime;
        
        Jump();
    }

    private void GroundCheck()
    {
        var raycastHit = Physics2D.Raycast(
            transform.position, 
            Vector2.down, 
            groundCheckDistance, 
            groundLayers);
        
        Debug.DrawRay(transform.position, 
            Vector2.down * groundCheckDistance, 
            Color.red);

        if (raycastHit)
        {
            if (!_grounded)
            {
                _grounded = true;
            } 
        }
        else
        {
            if (_grounded)
            {
                _grounded = false;
            }
        }
    }

    private void WallCheck()
    {
        var raycastHit = Physics2D.Raycast(
            transform.position, 
            Vector2.right * _playerInput.move.x, 
            wallCheckDistance, 
            wallLayers);
        
        Debug.DrawRay(transform.position, 
            Vector2.right * _playerInput.move.x * wallCheckDistance, 
            Color.red);

        if (raycastHit)
        {
            if (!_isClimbing && _jumpTimeOutElapsed < 0)
            {
                _isClimbing = true;
                _isClimbingLeftWall = _playerInput.move.x < 0;
            }
        }

        if (_isClimbing)
        {
            var hit = Physics2D.Raycast(
                transform.position, 
                Vector2.right * (_isClimbingLeftWall ? -1 : 1), 
                wallCheckDistance, 
                wallLayers);

            if (!hit)
            {
                _isClimbing = false;
            }
        }
    }

    private void MoveHorizontally()
    {
        if (_playerInput.move.x != 0)
        {
            if (_grounded || _isClimbing)
            {
                _speed += Time.fixedDeltaTime * _playerInput.move.x * accel;
            }
            else
            {
                if ((_playerInput.move.x > 0 && _speed < 0) || (_playerInput.move.x < 0 && _speed > 0))
                {
                    _speed += Time.fixedDeltaTime * _playerInput.move.x * airSpeedDecrease;
                    
                    if (_speed < setSpeedZeroOffset && _speed > -setSpeedZeroOffset)
                    {
                        _speed = 0;
                    }
                } 
            }
        }
        else
        {
            if (_grounded || _isClimbing) _speed -= Time.fixedDeltaTime * accel * (_speed > 0 ? 1 : -1);
            if (_speed < setSpeedZeroOffset && _speed > -setSpeedZeroOffset)
            {
                _speed = 0;
            }

        }

        
        _speed = Mathf.Clamp(_speed, -maxSpeed, maxSpeed);
    }

    private void ApplyActualMove()
    {
        if (_isClimbing)
        {
            _rigidbody.velocity = new Vector2(0, _speed * (_isClimbingLeftWall ? -1 : 1));
        }
        else
        {
            _rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
        }
}

    private void Jump()
    {
        if (_grounded || _isClimbing)
        {
            if (_jumpTimeOutElapsed < 0 && _playerInput.jump)
            {
                if (!_isClimbing)
                {
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                }
                else
                {
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(
                        Vector2.up * jumpPower,
                        ForceMode2D.Impulse);
                    _speed = _isClimbingLeftWall ? wallJumpPower : -wallJumpPower;
                    _isClimbing = false;
                }
            }
        }
    }
}
