using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private float SpawnRadius;
    [SerializeField] private Transform center;
    [SerializeField] private Transform _player;

    private List<Transform> enemyPos = new List<Transform>();
    private float _distanse;
    private float _closestToPlayerDist;
    public Transform _closietsEnemy { get; private set; }
    private bool _enemyMoveFrame;

    // only for test
    private float enemySpeed=5; 
    

    public void AddTransformToList(Transform transform)
    {
        
        enemyPos.Add(transform);
    }

    private void Awake()
    {
        instance = this;
    }

    [Serializable]
    class Wave
    {
        public float spawnTime;
        public Enemy enemyPrefab;
        public int countMin, countMax;
    }

    [SerializeField] private Wave[] waves;

    private int waveIndex;

    private Wave CurrentWave => waves[waveIndex];

    private bool HasWaves => waveIndex < waves.Length;

    private void FixedUpdate()
    {
        if (HasWaves && Time.timeSinceLevelLoad > CurrentWave.spawnTime)
        {
            Spawn();
            waveIndex++;
        }

        MoveEnemies();
    }

    public void FindClosiestEnemy()
    {
        _closestToPlayerDist=Int32.MaxValue;
        _closietsEnemy = null;
        enemyPos.RemoveAll(t => t == null);
        foreach (Transform t in enemyPos)
        {
            if (t != null)
            {
                _distanse = Vector3.Distance(t.transform.position, _player.transform.position);
            }

            if(_distanse<_closestToPlayerDist)
            {
                _closestToPlayerDist = _distanse;
                _closietsEnemy = t;
            }
        }         
    }   
    
    private void MoveEnemies()
    {
        _enemyMoveFrame = !_enemyMoveFrame;
        if(!_enemyMoveFrame)
        {
            return;
        }
        foreach (Transform t in enemyPos)
        {
            if(t != null)
            {
                Rigidbody2D rigidbody2D = t.GetComponent<Rigidbody2D>();
                Vector2 direction = (_player.position - t.position).normalized;
                //t.position += direction * 2 * Time.deltaTime
                rigidbody2D.MovePosition(rigidbody2D.position + (direction*Time.deltaTime*UnityEngine.Random.Range(0.3f, 1f) * enemySpeed));
            } 
        }
    }

    private void Spawn()
    {
        int count = UnityEngine.Random.Range(CurrentWave.countMin, CurrentWave.countMax + 1);

        // Получаем размеры камеры
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // Получаем позицию центра камеры
        Vector3 camPosition = cam.transform.position;

        for (int i = 0; i < count; i++)
        {
            // Случайно выбираем одну из четырех сторон
            int side = UnityEngine.Random.Range(0, 4);
            Vector3 position = Vector3.zero;

            switch (side)
            {
                case 0: 
                    position = new Vector3(UnityEngine.Random.Range(camPosition.x - camWidth / 2, camPosition.x + camWidth / 2), camPosition.y + camHeight / 2 + SpawnRadius, 0);
                    break;
                case 1: 
                    position = new Vector3(UnityEngine.Random.Range(camPosition.x - camWidth / 2, camPosition.x + camWidth / 2), camPosition.y - camHeight / 2 - SpawnRadius, 0);
                    break;
                case 2: 
                    position = new Vector3(camPosition.x - camWidth / 2 - SpawnRadius, UnityEngine.Random.Range(camPosition.y - camHeight / 2, camPosition.y + camHeight / 2), 0);
                    break;
                case 3: 
                    position = new Vector3(camPosition.x + camWidth / 2 + SpawnRadius, UnityEngine.Random.Range(camPosition.y - camHeight / 2, camPosition.y + camHeight / 2), 0);
                    break;
            }

            Instantiate(CurrentWave.enemyPrefab, position, Quaternion.identity);
        }
    }

}
