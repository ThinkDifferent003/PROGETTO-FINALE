using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _forceRecoil = 0.5f;
    [SerializeField] private float _durationRecoil = 0.2f;
    [SerializeField] private float _timerRecoil = 0f;
    
    private PlayerAnimation _playerAnimation;
    private Rigidbody _rb;
    private Vector3 _input;
    private Vector3 _lastMoveDirection = Vector3.back;
    private bool _isRecoiling = false;
   
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        
    }
    private void Update()
    {
        if (_isRecoiling) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        _input = new Vector3(moveX, 0, moveZ).normalized;
        if (_playerAnimation != null )
        {
            _playerAnimation.AnimationMovement(_input.x,_input.z);
        }
        if (_input.sqrMagnitude > 0 )
        {
            _lastMoveDirection = _input;
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
        if (_isRecoiling) return;

        Vector3 movement = new Vector3 (_input.x,0, _input.z) * _moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position +  movement);
    }
    public IEnumerator Recoil(Vector3 direction)
    {
        _isRecoiling = true;
        while (_timerRecoil < _durationRecoil)
        {
            _rb.MovePosition(_rb.position + direction * _forceRecoil * Time.deltaTime);
            _timerRecoil += Time.deltaTime;
            yield return null;
        }
        _isRecoiling = false;
        
    }
    public Vector3 GetLastDirection()
    {
        return _lastMoveDirection;
    }
   
}
