using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    // player's setting
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxWallSpeed;
    [SerializeField] private float accel;
    [SerializeField] private float jumpPower;
    [SerializeField] private float wallJumpPower;

    [SerializeField] private float airSpeedDecrease;

    [SerializeField] private float setSpeedZeroOffset;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayers;
    
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask wallLayers;

    [SerializeField] private float coyoteTime;
    
    [SerializeField] private float jumpTimeOut;
    [SerializeField] private float wallClimbTimeOut;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxHp;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private float hpSliderChangeSpeed;

    [SerializeField] private float noDamageTime;

    [SerializeField] private float waterConstant;

    [SerializeField] private float boilingInitialDamage;
    [SerializeField] private float boilingDOT;

    [SerializeField] private float maxVignetteIntensity, minVignetteIntensity;

    [SerializeField] private Volume postProcessing;
     

    // player's current state
    public float _speed;

    public bool _grounded;
    public bool _isClimbing;

    public float _coyoteTimeElapsed;
    
    public float _jumpTimeOutElapsed;
    public float _wallClimbTimeOutElapsed;
    
    public bool _isClimbingLeftWall;

    public float _hp;

    public float _noDamageTimeElapsed;

    public bool _getHitInitially = false;
    public bool gettingHit;

    public float _vignetteIntensity;
    public bool _vignetteIncrease;
    
    

    private Vignette _vignette;
    
    // components;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private PlayerInputManager _playerInput;

    private SpriteRenderer[] _renderers;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInputManager>();

        _renderers = GetComponentsInChildren<SpriteRenderer>();

        if (postProcessing.profile.TryGet(out _vignette))
        {
            _vignette.active = true;
        }
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
        _noDamageTimeElapsed -= Time.deltaTime;
        _coyoteTimeElapsed -= Time.deltaTime;

        Jump();
        RotateBody();
        HpSliderUpdate();
        DamageCheck();
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
            _coyoteTimeElapsed = coyoteTime;
            if (!_grounded)
            {
                _grounded = true;
                if (_isClimbing) _isClimbing = false;
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

            if (_playerInput.move.x != 0) _wallClimbTimeOutElapsed = wallClimbTimeOut;
            _wallClimbTimeOutElapsed -= Time.fixedDeltaTime;
            
            if (!hit || (_playerInput.move.x == 0 && _wallClimbTimeOutElapsed < 0))
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
                _speed += Time.fixedDeltaTime * _playerInput.move.x * airSpeedDecrease;
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

        float max = _isClimbing ? maxWallSpeed : maxSpeed;
        _speed = Mathf.Clamp(_speed, -max, max);
    }

    private void ApplyActualMove()
    {
        if (_isClimbing)
        {
            _rigidbody.velocity = new Vector2((_isClimbingLeftWall ? -1 : 1), _speed * (_isClimbingLeftWall ? -1 : 1));
        }
        else
        {
            _rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
            if (gettingHit && _rigidbody.velocity.y < 0) _rigidbody.velocity += Vector2.up * waterConstant;
        }
}

    private void Jump()
    {
        float actualJumpPower = gettingHit ? jumpPower / 3 : jumpPower;
        float actualWallJumpPower = gettingHit ? wallJumpPower / 3 : wallJumpPower;
        if (_coyoteTimeElapsed > 0 || _isClimbing)
        {
            if (_jumpTimeOutElapsed < 0 && _playerInput.jump)
            {
                if (!_isClimbing)
                {
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(Vector2.up * actualJumpPower, ForceMode2D.Impulse);
                }
                else
                {
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(
                        Vector2.up * actualJumpPower,
                        ForceMode2D.Impulse);
                    _speed = _isClimbingLeftWall ? actualWallJumpPower : -actualWallJumpPower;
                    _isClimbing = false;
                }
            }
        }
    }

    private void RotateBody()
    {
        float targetAngle = 0;
        if (!_isClimbing)
        {
            targetAngle = 0;
        }
        else
        {
            targetAngle = _isClimbingLeftWall ? -90 : 90;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);
    }

    private void HpSliderUpdate()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, _hp / maxHp, hpSliderChangeSpeed * Time.deltaTime);
    }

    private void DamageCheck()
    {
        if (gettingHit)
        {
            if (_getHitInitially)
            {
                if (_noDamageTimeElapsed < 0)
                {
                    _hp += boilingInitialDamage;
                    _getHitInitially = false;
                    _noDamageTimeElapsed = noDamageTime;

                    
                    // JUMP!
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                    _isClimbing = false;
                    
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);

                    _vignetteIncrease = true;
                    
                    // FLASH!
                    StartCoroutine(HitFlash());
                }
            } else if (_noDamageTimeElapsed < 0)
            {
                _hp += boilingDOT * Time.deltaTime;
                
                // post-processing
                if (_vignetteIncrease)
                {
                    _vignetteIntensity += Time.deltaTime;
                }
                else
                {
                    _vignetteIntensity -= Time.deltaTime;
                }
                
                if (_vignetteIntensity > maxVignetteIntensity)
                {
                    _vignetteIncrease = false;
                }
                
                if (_vignetteIntensity < minVignetteIntensity)
                {
                    _vignetteIncrease = true;
                }
                _vignette.intensity.Override(_vignetteIntensity);
            }
        } else
        {
            if (_noDamageTimeElapsed < 0)
            {
                _getHitInitially = true;
                _vignette.intensity.Override(0);
            }
            
        }
    }

    IEnumerator HitFlash()
    {
        float time = 0;
        int a = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            a++;

            if (a < 5)
            {
                foreach (var sprite in _renderers)
                {
                    sprite.color = Color.red;
                }
            }
            else
            {
                foreach (var sprite in _renderers)
                {
                    sprite.color = Color.white;
                }
            }

            if (time < 0.3f)
            {
                _vignette.intensity.Override(1f * time);
            }
            else if (time < 0.6f)
            {
                _vignette.intensity.Override(1 * (0.6f - time));
            }
            else if (time < 0.65f)
            {
                _vignette.intensity.Override(0);
            }

            if (a > 10) a = 0;
            yield return null;
            
            foreach (var sprite in _renderers)
            {
                sprite.color = Color.white;
            }
        }
    }
}
