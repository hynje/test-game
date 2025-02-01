using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // UI 
    [SerializeField] private TextMeshProUGUI gasText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private MoveButton leftMoveButton;
    [SerializeField] private MoveButton rightMoveButton;
    [SerializeField] private GameObject gameOverScreen;
    // Prefab
    [SerializeField] private GameObject gasPrefab;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject carPrefab;
    
    // State
    public enum State
    {
        Start,
        Play,
        End
    };

    public State GameState { get; private set; } = State.Start;

    private CarController _carController;
    
    // Road ObjectPool
    private Queue<GameObject> _roadPool = new Queue<GameObject>();
    private int _roadPoolSize = 5;
    
    // Road List
    private List<GameObject> _activeRoads = new List<GameObject>();
    
    // Gas ObjectPool
    private Queue<GameObject> _gasPool = new Queue<GameObject>();
    private int _gasPoolSize = 10;
    
    // Gas List
    private List<GameObject> _activeGas = new List<GameObject>();
    
    private int leftGas;
    private float _gasSpawnRate = 1.5f;
    //public bool isGameActive = false;
    private int[] spawnXPoints = { -1, 0, 1 };
    
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        InitializeRoadPool();
        InitializeGasPool();
    }


    private void InitializeRoadPool()
    {
        for (int i = 0; i < _roadPoolSize; i++)
        {
            GameObject road = Instantiate(roadPrefab);
            road.SetActive(false);
            _roadPool.Enqueue(road);
        }
    }

    public void SpawnRoad(Vector3 position)
    {
        if (_roadPool.Count > 0)
        {
            var road = _roadPool.Dequeue();
            road.transform.position = position;
            road.SetActive(true);
            _activeRoads.Add(road);
        }
        else
        {
            GameObject road = Instantiate(roadPrefab, position, Quaternion.identity);
            _activeRoads.Add(road);
        }
    }

    public void DestroyRoad(GameObject road)
    {
        road.SetActive(false);
        _activeRoads.Remove(road);
        _roadPool.Enqueue(road);
    }
    
    IEnumerator UpdateGas()
    {
        while (leftGas > 0 && GameState == State.Play)
        {
            yield return new WaitForSeconds(1);
            leftGas -= 10;
            gasText.text = leftGas.ToString();
            if (leftGas > 0) continue;
            GameOver();
            break;
        }
    }

    public void GetGas()
    {
        leftGas += 30;
        gasText.text = leftGas.ToString();
    }

    private void InitializeGasPool()
    {
        for (int i = 0; i < _gasPoolSize; i++)
        {
            GameObject gas = Instantiate(gasPrefab);
            gas.SetActive(false);
            _gasPool.Enqueue(gas);
        }
    }

    IEnumerator SpawnGas()
    {
        int spawnCount = 0;
        while (GameState == State.Play)
        {
            yield return new WaitForSeconds(_gasSpawnRate);
            var spawnXIndex = Random.Range(0, spawnXPoints.Length);
            Vector3 spawnPosition = new Vector3(spawnXPoints[spawnXIndex], 0.21f, 4);
            
            var gas = _gasPool.Dequeue();
            _activeGas.Add(gas);
            gas.transform.position = spawnPosition;
            gas.SetActive(true);
            
            spawnCount++;
            if (spawnCount == 4)
            {
                _gasSpawnRate += 0.12345f;
                spawnCount = 0;
            }
        }
    }

    public void DestroyGas(GameObject gas)
    {
        gas.SetActive(false);
        _activeGas.Remove(gas);
        _gasPool.Enqueue(gas);
    }

    public void StartGame()
    {
        GameState = State.Play;
        
        titleScreen.SetActive(false);
        gameScreen.SetActive(true);

        SpawnRoad(Vector3.zero);
        _carController = Instantiate(carPrefab, new Vector3(0, 0, -3f), Quaternion.identity)
            .GetComponent<CarController>();

        leftGas = _carController.maxGas;
        gasText.text = leftGas.ToString();
        leftMoveButton.OnMoveButtonPressed += () => _carController.Move(-1f);
        rightMoveButton.OnMoveButtonPressed += () => _carController.Move(1f);
        
        StartCoroutine(UpdateGas());
        StartCoroutine(SpawnGas());
    }

    private void GameOver()
    {
        GameState = State.End;
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void LoadTitleScreen()
    {
        GameState = State.Start;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
