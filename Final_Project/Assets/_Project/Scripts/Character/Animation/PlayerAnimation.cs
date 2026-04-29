using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private string _moveX = "MoveX";
    [SerializeField] private string _moveZ = "MoveZ";
    [SerializeField] private string _isMoving = "IsMoving";
    [SerializeField] private string _attack = "Attack";
    [SerializeField] private string _isDeath = "IsDeath";
    [SerializeField] private string _isHurt = "IsHurt";
     
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        
    }
    public void AnimationMovement(float x , float z)
    {
        if (_animator == null) return; //Se non c'è animator esco
        bool moving = (x != 0 || z != 0); //Se X o Z sono diversi da 0, il player si sta muovendo
        _animator.SetBool(_isMoving, moving); //Dico all'animator se attivare lo stato di movimento
        //Se si muove , aggiorno i valori degli assi
        if (moving)
        {
            _animator.SetFloat(_moveX, x);
            _animator.SetFloat(_moveZ, z);
        }
    }
    public void AnimationAttack()
    {
        if ( _animator == null) return;
        _animator.SetTrigger(_attack); //Attivo il trigger dell' attacco
    }
    public void AnimationDie()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_isDeath);//Attivo il trigger della morte
        }
    }
    public void AnimationHurt()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_isHurt);//Attiva il trigger di stordimento
        }
        
    }
}
