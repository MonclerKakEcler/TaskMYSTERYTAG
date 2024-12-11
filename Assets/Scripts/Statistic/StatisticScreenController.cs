using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StatisticScreenController
{
    private const string kStatisticItemPath = "Statistic/LastGameItem";

    private Transform _parent;
    private List<IStatisticItem> _statisticItems = new();

    private IStatisticScreenView _statisticScreenView;
    private IStatisticService _statisticService;
    private DiContainer _diContainer;

    public StatisticScreenController(DiContainer diContainer, IStatisticService statisticService)
    {
        _diContainer = diContainer;
        _statisticService = statisticService;
    }

    public void Init(IStatisticScreenView statisticScreenView)
    {
        _statisticScreenView = statisticScreenView;
        _parent = _statisticScreenView.ParentLastGames;

        CreateStatisticItems();
    }

    private void CreateStatisticItems()
    {
        _statisticItems.Clear();
        HelperUtils.ClearChildren(_parent.gameObject);

        GameObject prefab = Resources.Load<GameObject>(kStatisticItemPath);
        var statisticDataList = _statisticService.GetAllStatistics();

        int countToDisplay = Mathf.Min(10, statisticDataList.Count);
        for (int i = statisticDataList.Count - countToDisplay; i < statisticDataList.Count; i++)
        {
            GameObject instance = _diContainer.InstantiatePrefab(prefab, _parent);
            IStatisticItem item = instance.GetComponent<IStatisticItem>();

            var data = statisticDataList[i];
            item.ActivateRedImage(data.Win);
            item.SetLevelText("Level:" + data.LevelID.ToString());
            item.SetTimeText("Time: " + data.Time);
            item.SetScoreText("Score: " + data.Score.ToString());

            _statisticItems.Add(item);
        }
    }
}
