using System;
using UnityEngine;

public interface IShipView
{
    Action IsDamageTaken { get; set; }
    void WinScreen(bool isActive);
    void GameOverScreen(bool isActive);
}

public class ShipView : MonoBehaviour, IShipView
{
    [SerializeField] private RectTransform _shipTransform;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private float _speed;

    [Header("Screens")]
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _gameOverScreen;

    public Action IsDamageTaken { get; set; }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        if (direction == Vector2.zero) return;
        direction = direction.normalized;
        _shipTransform.anchoredPosition += direction * _speed * Time.fixedDeltaTime;
    }

    public void WinScreen(bool isActive)
    {
        _winScreen.SetActive(isActive);
    }

    public void GameOverScreen(bool isActive)
    {
        _gameOverScreen.SetActive(isActive);
    }
}