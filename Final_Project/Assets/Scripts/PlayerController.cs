using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private PlayerAnimation _playerAnimation;
    private Rigidbody _rb;
    private Vector3 _input;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        _input = new Vector3(moveX, 0, moveZ).normalized;
        if (_playerAnimation != null )
        {
            _playerAnimation.AnimationMovement(_input.x,_input.z);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (_playerAnimation != null)
            {
                _playerAnimation.AnimationAttack();
            }
        }
    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3 (_input.x,0, _input.z) * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position +  movement);
    }
}
