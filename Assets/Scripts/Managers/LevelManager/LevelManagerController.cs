using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = System.Random;

public class LevelManagerController
{
    private const string kBulletPath = "Bullets/Bullet";
    private const string kAsteroidPath = "Asteroids/VariantsAsteroids/{0}";
    private const string kMenuScene = "Menu";

    private string[] _asteroidNames =
    {
        "BlueAsteroid Variant",
        "BrownAsteroid Variant",
        "GreenLineAsteroid Variant",
        "YellowAsteroid Variant"
    };

    private int[] _randomIndexesAsteroids;
    private bool _isGameRunning;
    private int _currentLevel;

    private readonly int _sizePoolBullets = 10;
    private readonly int _sizePoolAsteroids = 10;

    private Transform _bulletsPoolParent;
    private List<Transform> _asteroidsSpawnPoints;

    private List<BulletItem> _bulletsItems = new();
    private List<AsteroidItem> _asteroidsItems = new();

    private List<LevelData> _levelData = new ();

    private DiContainer _diContainer;
    private ILevelManagerView _levelManagerView;
    private IObjectPool<BulletItem> _bulletPool;
    private ShipController _shipController;
    private ILevelService _levelService;

    private Random _random;

    [Inject]
    public LevelManagerController(
        IObjectPool<BulletItem> bulletPool,
        DiContainer diContainer,
        ShipController shipController,
        ILevelService levelService)
    {
        _bulletPool = bulletPool;
        _diContainer = diContainer;
        _shipController = shipController;
        _levelService = levelService;

        _random = new Random();
    }

    public void Init(ILevelManagerView levelManagerView)
    {
        _levelData.Clear();

        _levelManagerView = levelManagerView;

        _bulletsPoolParent = _levelManagerView.BulletsPoolParent;
        _asteroidsSpawnPoints = _levelManagerView.AsteroidPoint;

        _shipController.IsGameStop -= StopGame;
        _shipController.IsGameStop += StopGame;

        _levelManagerView.OnCloseClick -= CloseGameScene;
        _levelManagerView.OnCloseClick += CloseGameScene;

        _levelData = _levelService.GetAllLevelData();

        for (int i = 0; i < _levelData.Count; i++)
        {
            if (!_levelData[i].LevelComplete)
            {
                _currentLevel = i; 
                break;
            }
        }
        StartGame();
    }

    private void StartGame()
    {
        _isGameRunning = true;

        _bulletsItems = _bulletPool.InitializePool(_bulletsPoolParent, kBulletPath, _sizePoolBullets);

        GenerateRandomIndexesForAsteroids();
        CreateAsteroidsItems();

        BulletShooting().Forget();
        AsteroidsShooting().Forget();
    }

    private void CreateAsteroidsItems()
    {
        for (int i = 0; i < _sizePoolAsteroids; i++)
        {
            string asteroidName = _asteroidNames[_randomIndexesAsteroids[i]];

            string asteroidPath = string.Format(kAsteroidPath, asteroidName);
            GameObject prefab = Resources.Load<GameObject>(asteroidPath);

            GameObject instance = _diContainer.InstantiatePrefab(prefab, _levelManagerView.AsteroidsPoolParent);
            AsteroidItem item = instance.GetComponent<AsteroidItem>();

            item.StateAsteroid(false);
            _asteroidsItems.Add(item);
        }
    }

    private async UniTaskVoid BulletShooting()
    {
        while (_isGameRunning)
        {
            ShootBullet();
            await UniTask.Delay(_levelData[_currentLevel].SpeedBullet);
        }
    }

    private async UniTaskVoid AsteroidsShooting()
    {
        while (_isGameRunning)
        {
            ShootAsteroids();
            await UniTask.Delay(_levelData[_currentLevel].SpeedAsteroids);
        }
    }

    private void ShootBullet()
    {
        foreach (var bullet in _bulletsItems)
        {
            if (!bullet.IsActive)
            {
                bullet.Activate(_levelManagerView.FirePoint.position);
                return;
            }
        }
    }

    private void ShootAsteroids()
    {
        foreach (var asteroid in _asteroidsItems)
        {
            if (!asteroid.IsActive)
            {
                int randomSpawnIndex = _random.Next(0, _asteroidsSpawnPoints.Count);
                Transform spawnPoint = _asteroidsSpawnPoints[randomSpawnIndex];
                asteroid.Activate(spawnPoint.position);
                return;
            }
        }
    }

    private void GenerateRandomIndexesForAsteroids()
    {
        _randomIndexesAsteroids = new int[10];
        for (int i = 0; i < _randomIndexesAsteroids.Length; i++)
        {
            _randomIndexesAsteroids[i] = _random.Next(0, 4);
        }
    }

    private void StopGame()
    {
        _isGameRunning = false;

        _bulletsItems.Clear();
        _asteroidsItems.Clear();

        HelperUtils.ClearChildren(_bulletsPoolParent.gameObject);
        for (int i = 0; i < _asteroidsSpawnPoints.Count; i++)
        {
            HelperUtils.ClearChildren(_asteroidsSpawnPoints[i].gameObject);
        }
    }

    private void CloseGameScene()
    {
        StopGame();
        SceneManager.LoadScene(kMenuScene, LoadSceneMode.Single);
    }
}
