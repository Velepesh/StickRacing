using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickSpawner : MonoBehaviour
{
    [SerializeField] private Stick _stickTemplate;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _timeBetweenSpawn;

    private Stick _currentStick;

    public Stick CurrentStick => _currentStick;

    private void Awake()
    {
        if (_currentStick == null)
            _currentStick = Spawn();
    }

    public Stick SpawnNewStick()
    {
        _currentStick = Spawn();
    

        return _currentStick;
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(_timeBetweenSpawn);

        _currentStick = Spawn();
    }

    private Stick Spawn()
    {
        Stick stick = Instantiate(_stickTemplate.gameObject, _spawnPoint).GetComponent<Stick>();

        return stick;
    }
}
