using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IStatisticScreenView
{
    Transform ParentLastGames { get; }
}

public class StatisticScreenView : MonoBehaviour, IStatisticScreenView
{
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _parent;

    public Transform ParentLastGames => _parent;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseStatistic);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseStatistic);
    }

    private void CloseStatistic()
    {
        _playMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
