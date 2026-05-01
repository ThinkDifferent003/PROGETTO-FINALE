using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Animation Parameter Names")]
    [SerializeField] private string _moveX = "MoveX";
    [SerializeField] private string _moveZ = "MoveZ";
    [SerializeField] private string _isMoving = "IsMoving";
    [SerializeField] private string _attack = "Attack";
    [SerializeField] private string _isDeath = "IsDeath";
    [SerializeField] private string _isHurt = "IsHurt";
     
    private Animator _animator;

    private int _hashMoveX;
    private int _hashMoveZ;
    private int _hasMoving;
    private int _hashAttack;
    private int _hashDeath;
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
        _hasMoving = Animator.StringToHash(_isMoving);
        _hashAttack = Animator.StringToHash(_attack);
        _hashDeath = Animator.StringToHash(_isDeath);
        _hashHurt = Animator.StringToHash(_isHurt);
    }
    #region Animation Calls
    public void AnimationMovement(float x , float z)
    {
        if (_animator == null) return; //Se non c'è animator esco
        bool moving = (x != 0 || z != 0); //Se X o Z sono diversi da 0, il player si sta muovendo
        _animator.SetBool(_hasMoving, moving); //Dico all'animator se attivare lo stato di movimento
        //Se si muove , aggiorno i valori degli assi
        if (moving)
        {
            _animator.SetFloat(_moveX, x);
            _animator.SetFloat(_moveZ, z);
        }
    }
    public void AnimationAttack()
    {
        if ( _animator != null ) _animator.SetTrigger(_hashAttack); //Attivo il trigger dell' attacco
    }
    public void AnimationDie()
    {
        if (_animator != null) _animator.SetTrigger(_hashDeath);//Attivo il trigger della morte     
    }
    public void AnimationHurt()
    {
        if (_animator != null) _animator.SetTrigger(_hashHurt);//Attiva il trigger di stordimento     
    }
    #endregion
}
