using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string _moveX = "MoveX";
    [SerializeField] private string _moveZ = "MoveZ";
    [SerializeField] private string _isMoving = "IsMoving";
    [SerializeField] private string _attack = "Attack";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void AnimationMovement(float x , float z)
    {
        if (_animator == null) return;
        bool moving = (x != 0 || z != 0);
        _animator.SetBool(_isMoving, moving);
        if (moving)
        {
            _animator.SetFloat(_moveX, x);
            _animator.SetFloat(_moveZ, z);
        }
    }
    public void AnimationAttack()
    {
        if ( _animator == null) return;
        _animator.SetTrigger(_attack);
    }
}
