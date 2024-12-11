using UnityEngine;
using Zenject;
using Random = System.Random;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MenuManager _menuManager;
    private const string FirstLaunchKey = "FirstLaunch";

    private int _currentLevel;
    private Random _random;

    [Inject] ILevelService _levelService;
    [Inject] IStatisticService _statisticService;

    private void Start()
    {
        Init();
    }

    private async void Init()
    {
        await _statisticService.Init();
        if (IsFirstLaunch())
        {
            _random = new Random();
            await _levelService.Init();

            var level1Data = GenerateLevelData(1);
            var level2Data = GenerateLevelData(2);
            var level3Data = GenerateLevelData(3);
        }

        _menuManager.Init();
    }

    private LevelData GenerateLevelData(int id)
    {
        LevelData levelData = new LevelData { ID = id };

        switch (id)
        {
            case 1:
                levelData.CountAsteroids = _random.Next(3, 5);
                levelData.SpeedAsteroids = _random.Next(2000, 3000);
                levelData.SpeedBullet = _random.Next(200, 300); 
                _levelService.SaveLevelData(levelData.CountAsteroids, levelData.SpeedAsteroids, levelData.SpeedBullet, false);
                break;

            case 2:
                levelData.CountAsteroids = _random.Next(7, 12);
                levelData.SpeedAsteroids = _random.Next(1500, 2000);
                levelData.SpeedBullet = _random.Next(350, 500);
                _levelService.SaveLevelData(levelData.CountAsteroids, levelData.SpeedAsteroids, levelData.SpeedBullet, false);
                break;

            case 3:
                levelData.CountAsteroids = _random.Next(15, 20); 
                levelData.SpeedAsteroids = _random.Next(1000, 1500);
                levelData.SpeedBullet = _random.Next(500, 600);
                _levelService.SaveLevelData(levelData.CountAsteroids, levelData.SpeedAsteroids, levelData.SpeedBullet, false);
                break;
        }

        return levelData;
    }

    private bool IsFirstLaunch()
    {
        if (PlayerPrefs.GetInt(FirstLaunchKey, 0) == 0)
        {
            PlayerPrefs.SetInt(FirstLaunchKey, 1);
            PlayerPrefs.Save();
            return true;
        }

        return false;
    }
}
