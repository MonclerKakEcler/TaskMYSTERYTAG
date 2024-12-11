using System;
using System.Collections.Generic;

public class HeaderController 
{
    public event Action<int> OnHealthUpdated;

    private int _score;
    private int _currentLevel;

    private List<LevelData> _levelData;

    private IHeaderView _headerView;
    private IStatisticService _statisticService;
    private ILevelService _levelService;

    public HeaderController(IStatisticService statisticService, ILevelService levelService)
    {
        _statisticService = statisticService;
        _levelService = levelService;
    }

    public void Init(IHeaderView headerView)
    {
        var statisticDataList = _statisticService.GetAllStatistics();
        _headerView = headerView;

        AsteroidItem.OnAsteroidDestroyed -= AddScore;
        AsteroidItem.OnAsteroidDestroyed += AddScore;

        CheckCurrentLevel();
        UpdateScore(0);
        UpdateLevel(_currentLevel);
    }

    private void UpdateLevel(int level)
    {
        _headerView.SetLevelText("Lvl: " + _currentLevel);
    }

    public void UpdateHealth(int health)
    {
        OnHealthUpdated?.Invoke(health);
        _headerView.SetHealthText("Health: " + health);
    }

    private void UpdateScore(int score)
    {
        _score = score;
        _headerView.SetScoreText("Score: " + score);
    }

    private void AddScore()
    {
        _score += 5;
        UpdateScore(_score);
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
