using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;


public class PlayerInstantiator : MonoBehaviour
{
    // [SerializeField] private List<Checkpoint> _checkpoints;
    [SerializeField] private GameObject _playerPrefab;

    private GameObject _playerInstance;
    // private PlayerMove _playerMove;
    private CheckpointUpdaterService _checkpointUpdaterService;
    private ISceneModel _sceneModel;

    // private Checkpoint _currentCheckpoint;

    private int _currentCheckpointId, _initialCheckpointId = 0;

    // public PlayerMove Player
    // {
    //     get => _playerMove;
    //     private set => _playerMove = value;
    // }

    
    private int GetLatestCheckpoint()
    {
        return _checkpointUpdaterService.GetLatestCheckpoint(SceneUtils.GetCurrentSceneId());
    }

    //From Event
    public void UpdateLastCheckpoint(PlayerReachedCheckpointEvent playerReachedCheckpointEvent)
    {
        UpdateLastCheckpoint(playerReachedCheckpointEvent.Id);
    }

    // private void UpdateLastCheckpoint(Checkpoint checkpoint)
    // {
    //     UpdateLastCheckpoint(checkpoint.ID);
    // }

    private void UpdateLastCheckpoint(int checkpointID)
    {
        if (checkpointID <= _currentCheckpointId) return;
        _currentCheckpointId = checkpointID;
        
        _checkpointUpdaterService.UpdateLastCheckpoint(checkpointID, SceneUtils.GetCurrentSceneId());
    }

    public void ResetPlayerPosition()
    {
        // _playerInstance.transform.position = _checkpoints[_currentCheckpointId].transform.position;
        // _playerInstance.SetActive(true);
    }

    public Transform GetPlayerTransform()
    {
        return _playerInstance.transform;
    }

    private void Awake()
    {
        _checkpointUpdaterService = ServiceLocator.Instance.GetService<CheckpointUpdaterService>();
        _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
    }


    public void InstantiatePlayer()
    {
        // for (var index = 0; index < _checkpoints.Count; index++)
        // {
        //     var checkpoint = _checkpoints[index];
        //     checkpoint.Init(index);
        //     checkpoint.OnPlayerReachCheckpoint += UpdateLastCheckpoint;
        // }
        //
        // _currentCheckpointId = GetLatestCheckpoint();
        //
        // _playerInstance = Instantiate(_playerPrefab);
        // _playerInstance.SetActive(false);
        // _playerMove = _playerInstance.GetComponent<PlayerMove>();
    }
}