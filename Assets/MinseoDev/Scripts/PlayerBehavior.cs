using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
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

    [SerializeField] private Slider climbSlider;

    [SerializeField] private float noDamageTime;

    [SerializeField] private float waterConstant;

    [SerializeField] private float boilingInitialDamage;
    [SerializeField] private float boilingDOT;

    [SerializeField] private float maxVignetteIntensity, minVignetteIntensity;

    [SerializeField] private Volume postProcessing;

    [SerializeField] private Transform footPosition;

    [SerializeField] private float groundCheckTime;

    [SerializeField] private Camera camera;

    [Header("CLIMB GAUGE")] [SerializeField]
    private float maxClimbGauge;

    // player's current state
    public float _speed;

    public float _groundChekcTimeElapsed;
    public float _previousVelocityY;

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

    public bool _hitair;

    public float _climbGuage;
    
    public bool touchingDescendingPlatform = false;

    public int aniIdWalk;
    public int aniIdGrounded;
    public int aniIdJump;

    public bool pause;

    public GameObject currentDescendingPlatform;

    private Vignette _vignette;
    
    // components;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private PlayerInputManager _playerInput;
    private Animator _animator;
    private SpriteRenderer _sprite;

    private SpriteRenderer[] _renderers;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInputManager>();
        _animator = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();

        aniIdWalk = Animator.StringToHash("Walk");
        aniIdGrounded = Animator.StringToHash("Grounded");
        aniIdJump = Animator.StringToHash("Jump");

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
        float sizecamera = _hitair ? 4 : 5;
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, sizecamera, Time.deltaTime * 5);
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (_playerInput.pause)
        {
            pause = !pause;
            _playerInput.pause = false;
        }

        if (_isClimbing) _climbGuage -= Time.deltaTime;
        _climbGuage = Mathf.Clamp(_climbGuage, 0, maxClimbGauge);
        
        _jumpTimeOutElapsed -= Time.deltaTime;
        _noDamageTimeElapsed -= Time.deltaTime;
        _coyoteTimeElapsed -= Time.deltaTime;
        _groundChekcTimeElapsed -= Time.deltaTime;

        if (gettingHit) _coyoteTimeElapsed = coyoteTime;

        if (Mathf.Abs(_previousVelocityY - _rigidbody.velocity.y) > 0.1f)
        {
            _groundChekcTimeElapsed = groundCheckTime;
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("jump")) _animator.SetBool(aniIdJump, false);

        if (touchingDescendingPlatform)
        {
            Physics2D.IgnoreLayerCollision(3, 6, true); // true: 충돌 무시, false: 충돌 허용
        }
        else
        {
            Physics2D.IgnoreLayerCollision(3, 6, false);  // true: 충돌 무시, false: 충돌 허용
        }

        if ((_sprite.flipX && _playerInput.move.x > 0) || (!_sprite.flipX && _playerInput.move.x < 0))
            _sprite.flipX = !_sprite.flipX;
        _animator.SetFloat(aniIdWalk, _playerInput.move.x == 0? 0: 1);

        Jump();
        RotateBody();
        HpSliderUpdate();
        ClimbSliderUpdate();
        DamageCheck();
    }

    private void GroundCheck()
    {
        float a = 0;
        while (a < 1)
        {
            var raycastHit = Physics2D.Raycast(
                footPosition.position,
                Vector2.down,
                groundCheckDistance * a,
                groundLayers);
            a += 0.01f;

            if (raycastHit)
            {
                if (!_grounded && _rigidbody.velocity.y <= 0 && footPosition.position.y > raycastHit.collider.bounds.max.y)
                {
                    _coyoteTimeElapsed = coyoteTime;
                    _grounded = true;
                    _hitair = false;
                    if (_isClimbing) _isClimbing = false;

                    // 특수 기믹 확인
                    StartCoroutine(SpecialPlatformCheck(raycastHit));

                    // 애니메
                    if (_jumpTimeOutElapsed < 0) _animator.SetBool(aniIdGrounded, true);
                    _animator.SetBool(aniIdJump, false);
                }

                break;
            }
            
            else
            {
                if (_grounded)
                {
                    _grounded = false;

                    // 애니메
                    _animator.SetBool(aniIdGrounded, false);
                }

                if (_coyoteTimeElapsed < 0)
                {
                    if (currentDescendingPlatform) currentDescendingPlatform.layer = 6;
                    touchingDescendingPlatform = false;
                }
            }
        }
    }

    private IEnumerator SpecialPlatformCheck(RaycastHit2D hit)
    {
        BoxCollider2D box = hit.collider as BoxCollider2D;
        if (box != null)
        {
            // BoxCollider2D의 Bounds 가져오기
            Bounds bounds = box.bounds;

            // 충돌 지점
            Vector2 hitPoint = hit.point;

            // 외곽에 닿았는지 확인
            if (IsPointOnBoundsEdge(hitPoint, bounds))
            {
                if (hit.collider.GetComponent<VanshingPlatform>())
                {
                    hit.collider.GetComponent<VanshingPlatform>().Vanishing();
                }

                if (hit.collider.GetComponent<DescendingPlatform>())
                {
                    touchingDescendingPlatform = true;
                    var a = hit.collider.GetComponent<DescendingPlatform>();
                    currentDescendingPlatform = hit.transform.gameObject;
                    currentDescendingPlatform.layer = 8;
                    a.descending = true;
                }
                yield return new WaitForSeconds(groundCheckTime * 1.1f);
            }
            else
            {
                
            }
        }
    }
    
    private bool IsPointOnBoundsEdge(Vector2 point, Bounds bounds)
    {
        // Bounds의 엣지 근처에 있는지 확인
        float epsilon = 0.01f; // 작은 오차 허용값
        bool onLeftOrRightEdge = Mathf.Abs(point.x - bounds.min.x) < epsilon || Mathf.Abs(point.x - bounds.max.x) < epsilon;
        bool onTopOrBottomEdge = Mathf.Abs(point.y - bounds.min.y) < epsilon || Mathf.Abs(point.y - bounds.max.y) < epsilon;

        return onLeftOrRightEdge || onTopOrBottomEdge;
    }

    private void WallCheck()
    {
        var raycastHit = Physics2D.Raycast(
            transform.position, 
            Vector2.right * _playerInput.move.x, 
            wallCheckDistance, 
            wallLayers);
        
        if (raycastHit)
        {
            if (!_isClimbing && _jumpTimeOutElapsed < 0 && _climbGuage > 0)
            {
                _hitair = false;
                _isClimbing = true;
                _animator.SetBool(aniIdGrounded, true);
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
            
            if (!hit || (_playerInput.move.x == 0 && _wallClimbTimeOutElapsed < 0) || _climbGuage == 0)
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
                _speed += Time.fixedDeltaTime * _playerInput.move.x * (_hitair? 10 :airSpeedDecrease);
            }
            
        }
        else
        {
            if (_grounded || _isClimbing) 
                _speed = 0;
            if (!_hitair)
            {
                _speed = Mathf.Lerp(_speed, 0, Time.deltaTime * 5);
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
            if (gettingHit && _rigidbody.velocity.y < 0)
            {
                _rigidbody.velocity -= Vector2.up * _rigidbody.velocity.y * 0.7f;
                _rigidbody.velocity += Vector2.up * waterConstant;
            }

            if (touchingDescendingPlatform && _rigidbody.velocity.y >= 0)
            {
                
                _rigidbody.velocity -= Vector2.up * 2;
            }
        }
}

    private void Jump()
    {
        float actualJumpPower = gettingHit ? jumpPower / 2 : jumpPower;
        float actualWallJumpPower = gettingHit ? wallJumpPower / 2 : wallJumpPower;
        if (_coyoteTimeElapsed > 0 || _isClimbing)
        {
            if (_jumpTimeOutElapsed < 0 && _playerInput.jump)
            {
                if (!_isClimbing)
                {
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(Vector2.up * actualJumpPower, ForceMode2D.Impulse);
                    _playerInput.jump = false;
                    
                    _animator.SetBool(aniIdGrounded, false);
                    _animator.SetBool(aniIdJump, true);
                    touchingDescendingPlatform = false;
                    if (currentDescendingPlatform) currentDescendingPlatform.layer = 6;
                }
                else
                {
                    _climbGuage -= 0.3f;
                    _jumpTimeOutElapsed = jumpTimeOut;
                    _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
                    _rigidbody.AddForce(
                        Vector2.up * actualJumpPower,
                        ForceMode2D.Impulse);
                    _speed = _isClimbingLeftWall ? actualWallJumpPower : -actualWallJumpPower;
                    _isClimbing = false;
                    _playerInput.jump = false;
                    
                    touchingDescendingPlatform = false;
                    
                    _animator.SetBool(aniIdGrounded, false);
                    _animator.SetBool(aniIdJump, true);
                }
            }
        }
    }

    public void Jump(float power)
    {
        if (!_isClimbing)
        {
            _jumpTimeOutElapsed = jumpTimeOut;
            _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
            _rigidbody.AddForce(Vector2.up * power, ForceMode2D.Impulse);
            _playerInput.jump = false;
                    
            _animator.SetBool(aniIdGrounded, false);
            _animator.SetBool(aniIdJump, true);
            touchingDescendingPlatform = false;
            if (currentDescendingPlatform) currentDescendingPlatform.layer = 6;
        }
        else
        {
            _climbGuage -= 0.3f;
            _jumpTimeOutElapsed = jumpTimeOut;
            _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
            _rigidbody.AddForce(
                Vector2.up * power,
                ForceMode2D.Impulse);
            _speed = _isClimbingLeftWall ? power : -power;
            _isClimbing = false;
            _playerInput.jump = false;
                    
            touchingDescendingPlatform = false;
                    
            _animator.SetBool(aniIdGrounded, false);
            _animator.SetBool(aniIdJump, true);
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
    
    private void ClimbSliderUpdate()
    {
        climbSlider.value = Mathf.Lerp(climbSlider.value, _climbGuage / maxClimbGauge, hpSliderChangeSpeed * Time.deltaTime);
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

    public void SingleDamage(float damage)
    {
        _hp += damage;
        _noDamageTimeElapsed = noDamageTime;

                    
        // JUMP!
        _hitair = true; 
        
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _isClimbing = false;
                    
        _jumpTimeOutElapsed = jumpTimeOut;
        _rigidbody.velocity = Vector2.Scale(Vector2.right, _rigidbody.velocity);
        _rigidbody.AddForce(Vector2.up * jumpPower / 2, ForceMode2D.Impulse);
        _speed = _sprite.flipX ? 5 : -5;

        _vignetteIncrease = true;
        
        // FLASH!
        StartCoroutine(HitFlash());
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
