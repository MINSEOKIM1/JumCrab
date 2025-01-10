using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // player's setting
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accel;
    [SerializeField] private float jumpPower;

    // player's current state
    private float _speed;
    
    // components;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    
    
}
