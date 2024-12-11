using DG.Tweening;
using System;
using UnityEngine;

public class AsteroidItem : MonoBehaviour
{
    public static event Action OnAsteroidDestroyed;
    public bool IsActive => _isActive;

    private int _maxHP = 300;
    private int _currentHP;
    private float _speed = 3f;
    private float _distance = -12f;
    private bool _isActive;

    public void StateAsteroid(bool state)
    {
        _isActive = state;
        gameObject.SetActive(state);
    }

    public void Activate(Vector2 position)
    {
        _currentHP = _maxHP;
        transform.position = position;

        StateAsteroid(true);

        float targetY = transform.position.y + _distance;

        transform.DOMoveY(targetY, Mathf.Abs(_distance) / _speed)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Deactivate();
            });
    }

    public void TakeDamage(int damage)
    {
        if (_currentHP > 0)
        {
            _currentHP -= damage;
        }

        if (_currentHP <= 0)
        {
            OnAsteroidDestroyed?.Invoke();
            Deactivate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ShipView shipView))
        {
            shipView.IsDamageTaken?.Invoke();
            Deactivate();
        }
    }

    private void Deactivate()
    {
        transform.DOKill();
        StateAsteroid(false);
    }
}
