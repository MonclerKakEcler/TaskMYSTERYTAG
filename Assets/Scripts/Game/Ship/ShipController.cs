using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipController
{
    public Action IsGameStop { get; set; }

    private float _startTime;
    private float _endTime;

    private int _maxLives = 3;
    private int _currentLives;
    private int _destroyedAsteroidsCount = 0;
    private int _scoreAsteroids = 0;
    private int _currentLevel;
    private bool _isWin;

    private List<LevelData> _levelData = new ();
    private IShipView _shipView;
    private HeaderController _headerController;
    private IStatisticService _statisticService;
    private ILevelService _levelService;

    public ShipController(HeaderController headerController, IStatisticService statisticService, ILevelService levelService)
    {
        _headerController = headerController;
        _statisticService = statisticService;
        _levelService = levelService;
    }

    public void Init(IShipView shipView)
    {
        _shipView = shipView;

        _shipView.IsDamageTaken -= TakeDamage;
        _shipView.IsDamageTaken += TakeDamage;

        AsteroidItem.OnAsteroidDestroyed -= DestroyAsteroid;
        AsteroidItem.OnAsteroidDestroyed += DestroyAsteroid;

        _currentLives = _maxLives;
        _destroyedAsteroidsCount = 0;
        _scoreAsteroids = 0;

        _levelData = _levelService.GetAllLevelData();
        _headerController.UpdateHealth(_currentLives);
        StartTimer();
    }

    public void DestroyAsteroid()
    {
        _destroyedAsteroidsCount++;
        _scoreAsteroids += 5;

        if (_destroyedAsteroidsCount > _levelData[_currentLevel].CountAsteroids)
        {
            WinScreen();
        }
    }

    private void TakeDamage()
    {
        _currentLives -= 1;

        if (_currentLives <= 0)
        {
            _currentLives = 0;
            GameOverScreen();
        }

        _headerController.UpdateHealth(_currentLives);
    }

    private void GameOverScreen()
    {
        _isWin = false;
        StopTimer();
        IsGameStop?.Invoke();
        _shipView.GameOverScreen(true);

        SaveStatistic();
        _scoreAsteroids = 0;
    }

    private void WinScreen()
    {
        _isWin = true;
        StopTimer();
        IsGameStop?.Invoke();
        _shipView.WinScreen(true);

        _levelData[_currentLevel].LevelComplete = true;
        _levelService.SaveLevelID(true);

        SaveStatistic();
        _scoreAsteroids = 0;
    }

    private void StartTimer()
    {
        _startTime = Time.time;
    }

    private void StopTimer()
    {
        _endTime = Time.time;
    }

    private string GetElapsedTime()
    {
        float elapsedTime = _endTime - _startTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        return timeSpan.ToString(@"mm\:ss");
    }

    private void SaveStatistic()
    {
        _statisticService.SaveStatisticData(_currentLevel, GetElapsedTime(), _scoreAsteroids, _isWin);
    }

    private void CheckCurrentLevel()
    {
        _levelData = _levelService.GetAllLevelData();

        for (int i = 0; i < _levelData.Count; i++)
        {
            if (!_levelData[i].LevelComplete)
            {
                _currentLevel = i + 1;
                break;
            }
        }
    }
}
