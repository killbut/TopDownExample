using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameState : BaseState
{
    private Dictionary<GameObject, Transform> _prefabsPositions;
    
    private int _scorePlayer;
    private int _scoreEnemy;
    
    public GameState(StateMachine stateMachine, Level level) : base(stateMachine)
    {
        _prefabsPositions = new Dictionary<GameObject, Transform>();
       CreatePrefabs(level);
    }
    
    public override void Enter()
    {
        base.Enter();
        Bullet.OnHitPerson += IncreaseScore;
        Bullet.OnHitPerson += RestartGame;
        EnablePrefabs();
    }

    private void EnablePrefabs()
    {
        foreach (var item in _prefabsPositions)
        {
            item.Key.SetActive(true);
        }
    }


    public override void Exit()
    {
        base.Exit();
        Bullet.OnHitPerson -= IncreaseScore;
        Bullet.OnHitPerson -= RestartGame;
        DisablePrefabs();
        _scoreEnemy = 0;
        _scorePlayer = 0;
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Escape))
            _stateMachine.ChangeState(_stateMachine.States[typeof(MenuState)]);
    }

    public override void OnGUI()
    {
        base.OnGUI();
        GUI.Label(new Rect(10,10,300,300),$"Enemy:{_scoreEnemy.ToString()}\nPlayer:{_scorePlayer.ToString()}");
    }
    
    private void CreatePrefabs(Level level)
    {
        foreach (var item in level.SpawnSetting)
        {
            var prefab = item.Prefab;
            var theirSpawn = item.SpawnTransform;
            var ingamePrefab= GameObject.Instantiate(prefab, theirSpawn.position, Quaternion.identity, null);
            ingamePrefab.SetActive(false);
            _prefabsPositions.Add(ingamePrefab,theirSpawn);
        }
    }
    private void DisablePrefabs()
    {
        foreach (var item in _prefabsPositions)
        {
            item.Key.SetActive(false);
        }
    }
    private void IncreaseScore(GameObject obj)
    {
        if (obj.CompareTag("Player"))
            _scoreEnemy++;
        else 
            _scorePlayer++;
    }
    private void RestartGame(GameObject obj)
    {
        foreach (var item in _prefabsPositions)
        {
            var gameobject = item.Key;
            var spawn = item.Value;
            gameobject.transform.position = spawn.position;
            
        }
    }
}