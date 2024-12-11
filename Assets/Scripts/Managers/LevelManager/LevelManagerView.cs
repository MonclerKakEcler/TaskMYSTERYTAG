using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ILevelManagerView
{
    Transform BulletsPoolParent { get; }
    Transform AsteroidsPoolParent { get; }
    Transform FirePoint { get; }
    List<Transform> AsteroidPoint { get; }
    Action OnCloseClick { get; set; }
}

public class LevelManagerView : MonoBehaviour, ILevelManagerView
{
    [SerializeField] private Transform _bulletPoolParent;
    [SerializeField] private Transform _asteroidsPoolParent;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private List<Transform> _asteroidPoint;
    [SerializeField] private Button _closeButton;

    public Transform AsteroidsPoolParent => _asteroidsPoolParent;
    public Transform BulletsPoolParent => _bulletPoolParent;
    public Transform FirePoint => _firePoint;
    public List<Transform> AsteroidPoint => _asteroidPoint;

    public Action OnCloseClick { get; set; }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseButtonClisk);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseButtonClisk);
    }

    private void OnCloseButtonClisk()
    {
        OnCloseClick?.Invoke();
    }
}
