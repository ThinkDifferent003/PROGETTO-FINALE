using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f; //Velocità
    [SerializeField] private float _forceRecoil = 0.5f; //Forza della spinta durante il rinculo
    [SerializeField] private float _durationRecoil = 0.2f; //Durata del blocco
    [SerializeField] private float _timerRecoil = 0f; //Contatore della durata del rinculo
    
    private PlayerAnimation _playerAnimation;
    private Rigidbody _rb;
    private Vector3 _input;
    private Vector3 _lastMoveDirection = Vector3.back; // Direzione di "sguardo"
    private bool _isRecoiling = false; // Se il player sta subendo il colpo
   
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
        
    }
    private void Update()
    {
        if (_isRecoiling) return; // Se il player è in rinculo, esci dall'Update

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        _input = new Vector3(moveX, 0, moveZ).normalized; //Normalizzo per la diagonale
       
        //Se esiste lo script dell'animazione, passo i valori
        if (_playerAnimation != null ) _playerAnimation.AnimationMovement(_input.x,_input.z);
       
        //Se il player si sta muovendo salvo la direzione come ultima
        if (_input.sqrMagnitude > 0 ) _lastMoveDirection = _input;
       
        //Se premo il tasto sinistro del mouse
        if (Input.GetMouseButtonDown(0))
        {   //Faccio partire l'animazione di attacco
            if (_playerAnimation != null) _playerAnimation.AnimationAttack();
        }
     }
    private void FixedUpdate()
    {   
        if (_isRecoiling) return; //Se sta subendo rinculo , esco
        
        //Calcolo lo spostamento fisico:(      Direzione    )    (velocità)  (Tempo fisso di Unity)
        Vector3 movement = new Vector3 (_input.x,0, _input.z) * _moveSpeed * Time.fixedDeltaTime;
        
        //Muovo il rigidbody
        _rb.MovePosition(_rb.position +  movement);
    }
    public IEnumerator Recoil(Vector3 direction)
    {
        _isRecoiling = true; // Blocco i controlli del player
        _timerRecoil = 0f; //Reset del timer
        
        //Ciclo finchè il tempo trascorso è minore della durata impostata
        while (_timerRecoil < _durationRecoil)
        {    
            //Sposto il player nella direzione passata come argomento
            _rb.MovePosition(_rb.position + direction * _forceRecoil * Time.deltaTime);
            _timerRecoil += Time.deltaTime; //Incremento il timer
            yield return null; //Aspetto il frame successivo
        }
        _isRecoiling = false; //Sblocca i controlli del player
        
    }
    public Vector3 GetLastDirection()
    {
        return _lastMoveDirection;
    }
   
}
