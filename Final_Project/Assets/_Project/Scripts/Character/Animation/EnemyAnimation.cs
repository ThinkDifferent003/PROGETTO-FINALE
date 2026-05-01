using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("Animation Parameter Names")]
    [SerializeField] private string _moveX = "MoveX";
    [SerializeField] private string _moveZ = "MoveZ";
    [SerializeField] private string _isMoving = "IsMoving";
    [SerializeField] private string _attack = "Attack";
    [SerializeField] private string _die = "Die";
    [SerializeField] private string _isHurt = "IsHurt";

    private Animator _animator;

    private int _hashMoveX;
    private int _hashMoveZ;
    private int _hashMoving;
    private int _hashAttack;
    private int _hashDie;
    private int _hashHurt;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InizializeHashes();
    }
    private void InizializeHashes()
    {
        _hashMoveX = Animator.StringToHash(_moveX);
        _hashMoveZ = Animator.StringToHash(_moveZ);
        _hashMoving = Animator.StringToHash(_isMoving);
        _hashAttack = Animator.StringToHash(_attack);
        _hashDie = Animator.StringToHash(_die);
        _hashHurt = Animator.StringToHash(_isHurt);
    }
    #region Animation Calls
    public void SetMoves(float x, float z, bool moving)
    {
        if (_animator == null) return;
        _animator.SetFloat(_hashMoveX,x);
        _animator.SetFloat(_hashMoveZ,z);
        _animator.SetBool(_hashMoving,moving);
    }
    public void AnimationDie()
    {
        if (_animator != null) _animator.SetTrigger(_die);      
    }
    public void AnimationAttack()
    {
        if (_animator != null) _animator.SetTrigger(_attack);
    }
    public void AnimationHurt()
    {
        if (_animator != null) _animator.SetTrigger(_isHurt);
    }
    #endregion
}
