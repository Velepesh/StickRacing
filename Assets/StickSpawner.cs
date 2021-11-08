using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickSpawner : MonoBehaviour
{
    [SerializeField] private Stick _stickTemplate;
    [SerializeField] private UnityEngine.Transform _spawnPoint;
    [SerializeField] private float _timeBetweenSpawn;

    private Stick _currentStick;
    private List<Stick> _sticks = new List<Stick>();
    public Stick CurrentStick => _currentStick;

    public event UnityAction<Stick> StickConnected;

    private void Awake()
    {
        if (_currentStick == null)
            _currentStick = Spawn();
    }

    public void SpawnNewStick()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(_timeBetweenSpawn);

        _currentStick = Spawn();
        _sticks.Add(_currentStick);
    }

    private void OnDisable()
    {
        foreach (var stick in _sticks)
        {
            stick.Connected -= OnConnected;
        }
    }

    private void OnConnected(Stick stick)
    {
        StickConnected?.Invoke(stick);
    }

    private Stick Spawn()
    {
        Stick stick = Instantiate(_stickTemplate.gameObject, _spawnPoint).GetComponent<Stick>();
        stick.Connected += OnConnected;

        return stick;
    }
}
