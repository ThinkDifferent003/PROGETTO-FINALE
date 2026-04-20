using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private string _moveX = "MoveX";
    [SerializeField] private string _moveZ = "MoveZ";
    [SerializeField] private string _isMoving = "IsMoving";
    [SerializeField] private string _attack = "Attack";
    [SerializeField] private string _die = "Die";
    [SerializeField] private string _isHurt = "IsHurt";


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetMoves(float x, float z, bool moving)
    {
        if (_animator == null) return;
        _animator.SetFloat(_moveX,x);
        _animator.SetFloat(_moveZ,z);
        _animator.SetBool(_isMoving,moving);
    }
    public void AnimationDie()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_die);
        }
    }
    public void AnimationAttack()
    {
        if (_animator == null) return;
        _animator.SetTrigger(_attack);
    }
    public void AnimationHurt()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_isHurt);
        }
    }
}
