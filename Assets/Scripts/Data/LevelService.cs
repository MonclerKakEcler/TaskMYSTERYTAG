using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public interface ILevelService
{
    UniTask Init();
    void SaveLevelData(int countAsteroids, int speedAsteroids, int shipShots, bool LevelComplete);
    void SaveLevelID(bool levelComplete);
    LevelData GetLevelData(int id);
    List<LevelData> GetAllLevelData();
}

public class LevelService :  ILevelService
{
    private readonly ILevelProvider _levelProvider;
    private readonly Random _random;

    private LevelHolder _levelHolder;

    public LevelService()
    {
        var path = Application.persistentDataPath;
        _levelProvider = new LevelProvider(path);

        _random = new Random();
    }

    public async UniTask Init()
    {
        await _levelProvider.Init();
        _levelHolder = _levelProvider.GetData();
    }

    public LevelData GetLevelData(int id)
    {
        var levelData = _levelHolder.LevelDataList.FirstOrDefault(x => x.ID == id);

        if (levelData == null)
        {
            throw new ArgumentException($"No data for id: {id}");
        }

        return levelData;
    }

    public void SaveLevelData(int countAsteroids, int speedAsteroids, int speedBullet, bool levelComplete)
    {
        var id = _levelHolder.ID;
        var levelData = new LevelData()
        {
            ID = id,
            CountAsteroids = countAsteroids,
            SpeedAsteroids = speedAsteroids,
            SpeedBullet = speedBullet,
            LevelComplete = levelComplete
        };

        _levelHolder.LevelDataList.Add(levelData);
        _levelHolder.ID++;
        _levelProvider.Save(_levelHolder);
    }

    public void SaveLevelID(bool levelComplete)
    {
        var id = _levelHolder.ID;
        var levelData = new LevelData()
        {
            ID = id,
            LevelComplete = levelComplete
        };

        _levelHolder.LevelDataList.Add(levelData);
        _levelHolder.ID++;
        _levelProvider.Save(_levelHolder);
    }

    public List<LevelData> GetAllLevelData()
    {
        return _levelHolder.LevelDataList;
    }
}
