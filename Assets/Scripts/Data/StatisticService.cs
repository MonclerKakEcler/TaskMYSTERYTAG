using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using System;

public interface IStatisticService
{
    UniTask Init();
    StatisticData GetStatisticData(int id);
    void SaveStatisticData(int levelId, string time, int score, bool isWin);
    List<StatisticData> GetAllStatistics();
}

public class StatisticService : IStatisticService
{
    private readonly IStatisticProvider _statisticProvider;
    private readonly Random _random;

    private StatisticHolder _statisticHolder;

    public StatisticService()
    {
        var path = Application.persistentDataPath;
        _statisticProvider = new StatisticProvider(path);

        _random = new Random();
    }

    public async UniTask Init()
    {
        await _statisticProvider.Init();
        _statisticHolder = _statisticProvider.GetData();
    }

    public StatisticData GetStatisticData(int id)
    {
        var statisticData = _statisticHolder.StatisticDataList.FirstOrDefault(x => x.ID == id);

        if (statisticData == null)
        {
            throw new ArgumentException($"No data for id: {id}");
        }

        return statisticData;
    }

    public void SaveStatisticData(int levelId, string time, int score, bool win)
    {
        var id = _statisticHolder.ID;
        var statisticData = new StatisticData() 
        {
            ID = id,
            LevelID = levelId,
            Time = time,
            Score = score,
            Win = win,
        };

        _statisticHolder.StatisticDataList.Add(statisticData);
        _statisticHolder.ID++;
        _statisticProvider.Save(_statisticHolder);
    }

    public List<StatisticData> GetAllStatistics()
    {
        return _statisticHolder.StatisticDataList;
    }
}
