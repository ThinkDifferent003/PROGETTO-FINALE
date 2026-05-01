using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializerManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string _spawnName = "PlayerSpawn";
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Inizialize());
    }
    private IEnumerator Inizialize()
    {
        yield return new WaitForEndOfFrame();
        PositionManager.SpawnType spawnType = PositionManager.GetCurrentType();

        switch (spawnType)
        {
            case PositionManager.SpawnType.SAVE_POINT: SavePointload(); break;
            case PositionManager.SpawnType.PORTAL: PortalLoad(); break;
            case PositionManager.SpawnType.DEFAULT: 
            default: DefaultLoad(); break;
        }
        PositionManager.Clear();
    }
    #region Load
    private void SavePointload()
    {
        SaveManager.Instance.PerformLoad(true);
    }
    private void PortalLoad()
    {
        SpawnPortal(PositionManager.GetDetail());
        SaveManager.Instance.PerformLoad(false);
        var playerPortal = FindObjectOfType<PlayerLife>();
        if (playerPortal != null) playerPortal.UpdateUIExternally();
    }
    private void DefaultLoad()
    {
        SpawnDefault();
        SaveManager.Instance.PerformLoad(false);
        var player = FindObjectOfType<PlayerLife>();
        if (player != null)
        {
            int maxHP = player.GetMaxHealth();
            player.SetHealth(maxHP);
            if (SaveManager.Instance._data != null)
            {
                SaveManager.Instance._data._health = maxHP;
                SaveManager.Instance.SaveGame(SaveManager.Instance._data);
            }
        }
    }
    #endregion
    #region Position Logic
    private void SpawnPortal(string portalName)
    {
        GameObject targetPortal = GameObject.Find(portalName);
        if (targetPortal != null) MovePlayer(targetPortal.transform.position);
        else SpawnDefault();     
    }
    private void SpawnDefault()
    {
        GameObject defaultSpawn = GameObject.Find(_spawnName); //Trovo l'ggetto
        if (defaultSpawn != null) MovePlayer(defaultSpawn.transform.position); //Spawn in quella posizione    
    }
    private void MovePlayer(Vector3 pos)
    {
        var player = FindObjectOfType<PlayerLife>(); //Cerco l'oggetto con playerlife
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = pos;
            }
            player.transform.position = pos; //Sposto il player   
            player.Reset();
            var move = player.GetComponent<PlayerController>();
            if (move != null) move.enabled = true;
        }
    }
    #endregion
}
